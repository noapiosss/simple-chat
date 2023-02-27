using System.Net.WebSockets;
using System.Security.Claims;

namespace Api.Services
{
    public class SocketMiddleware
    {
        private readonly RequestDelegate _next;
        private SocketHandler SocketHandler { get; set; }

        public SocketMiddleware(RequestDelegate next, SocketHandler socketHandler)
        {
            _next = next;
            SocketHandler = socketHandler;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (!httpContext.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            WebSocket socket = await httpContext.WebSockets.AcceptWebSocketAsync();
            string username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            await SocketHandler.OnConnected(username, socket);
            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await SocketHandler.Receive(socket, result, buffer);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await SocketHandler.OnDisconnected(socket);
                }
            });
        }

        private static async Task Receive(WebSocket webSocket, Action<WebSocketReceiveResult, byte[]> messageHandler)
        {
            byte[] buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageHandler(result, buffer);
            }
        }
    }
}