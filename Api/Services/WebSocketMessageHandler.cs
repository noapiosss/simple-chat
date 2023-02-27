using System.Net.WebSockets;
using System.Text;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class WebScoketMessageHandler : SocketHandler
    {
        private readonly IMessageBuffer _messageBuffer;

        public WebScoketMessageHandler(ConnectionManager connections, IMessageBuffer messageBuffer) : base(connections)
        {
            _messageBuffer = messageBuffer;
        }

        public override async Task OnConnected(string username, WebSocket socket)
        {
            await base.OnConnected(username, socket);
            string message = $"<b style=\"color:green\">[{DateTime.Now:HH:mm:ss}]</b> <i><b>{username}</b> has connected</i>";

            await SendMessage(socket, $"messageBuffer:::{string.Join("&&&", _messageBuffer.GetMessages())}");

            _messageBuffer.AddMessage(message);
            await SendMessageToAll(message);

            await SendMessageToAll($"userlist:::{string.Join("&&&", GetUsernames())}");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            string username = Connections.GetUsername(socket);
            string message = $"<b style=\"color:red\">[{DateTime.Now:HH:mm:ss}]</b> <i><b>{username}</b> has disconnected</i>";

            _messageBuffer.AddMessage(message);
            await SendMessageToAll(message);

            await base.OnDisconnected(socket);

            await SendMessageToAll($"userlist:::{string.Join("&&&", GetUsernames())}");
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string username = Connections.GetUsername(socket);
            string message = $"<b style=\"color:blue\">[{DateTime.Now:HH:mm:ss}]</b> <b>{username}:</b> {Encoding.UTF8.GetString(buffer, 0, result.Count)}";

            _messageBuffer.AddMessage(message);
            await SendMessageToAll(message);
        }
    }
}