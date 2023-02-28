using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Api.Models;
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
            ChatMessage message = new()
            {
                Username = username,
                Time = $"{DateTime.Now:HH:mm:ss}",
                Message = "has connected",
                MessageType = "connectMessage"
            };

            await SendMessage(socket, $"{{\"bufferMessages\":{JsonSerializer.Serialize(_messageBuffer.GetMessages())}}}");

            _messageBuffer.AddMessage(message);
            await SendMessageToAll($"{{\"systemMessage\":{JsonSerializer.Serialize(message)}}}");

            await SendMessageToAll($"{{\"userList\":{JsonSerializer.Serialize(GetUsernames())}}}");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            string username = Connections.GetUsername(socket);
            ChatMessage message = new()
            {
                Username = username,
                Time = $"{DateTime.Now:HH:mm:ss}",
                Message = "has connected",
                MessageType = "disconnectMessage"
            };

            _messageBuffer.AddMessage(message);
            await SendMessageToAll($"{{\"systemMessage\":{JsonSerializer.Serialize(message)}}}");

            await base.OnDisconnected(socket);

            await SendMessageToAll($"{{\"userList\":{JsonSerializer.Serialize(GetUsernames())}}}");
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string username = Connections.GetUsername(socket);
            ChatMessage message = new()
            {
                Username = username,
                Time = $"{DateTime.Now:HH:mm:ss}",
                Message = Encoding.UTF8.GetString(buffer, 0, result.Count),
                MessageType = "userMessage"
            };

            _messageBuffer.AddMessage(message);
            await SendMessageToAll($"{{\"userMessage\":{JsonSerializer.Serialize(message)}}}");
        }
    }
}