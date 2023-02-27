using System.Net.WebSockets;
using System.Text;

namespace Api.Services
{
    public class WebScoketMessageHandler : SocketHandler
    {
        public WebScoketMessageHandler(ConnectionManager connections) : base(connections)
        {
        }

        public override async Task OnConnected(string username, WebSocket socket)
        {
            await base.OnConnected(username, socket);
            await SendMessageToAll($"<b style=\"color:green\">[{DateTime.Now:HH:mm:ss}]</b> <b>{username}</b> connected");

            await SendMessageToAll($"userlist:{string.Join("&&&", GetUsernames())}");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            string username = Connections.GetUsername(socket);
            await SendMessageToAll($"<b style=\"color:red\">[{DateTime.Now:HH:mm:ss}]</b> <b>{username}</b> disconnected");
            await base.OnDisconnected(socket);

            await SendMessageToAll($"userlist:{string.Join("&&&", GetUsernames())}");
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string username = Connections.GetUsername(socket);
            string message = $"<b style=\"color:blue\">[{DateTime.Now:HH:mm:ss}]</b> <b>{username}:</b> {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            await SendMessageToAll(message);
        }
    }
}