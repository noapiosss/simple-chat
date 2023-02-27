using System.Net.WebSockets;
using System.Text;

namespace Api.Services
{
    public abstract class SocketHandler
    {
        public ConnectionManager Connections { get; set; }

        public SocketHandler(ConnectionManager connections)
        {
            Connections = connections;
        }

        public virtual async Task OnConnected(string username, WebSocket socket)
        {
            await Task.Run(() => { Connections.AddSocket(username, socket); });
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await Connections.RemoveSocketAsync(Connections.GetUsername(socket));
        }

        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
            {
                return;
            }

            await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0, message.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task SendMessage(string username, string message)
        {
            await SendMessage(Connections.GetSocketByUsername(username), message);
        }

        public async Task SendMessageToAll(string message)
        {
            foreach (KeyValuePair<string, WebSocket> connection in Connections.GetAllConnections())
            {
                await SendMessage(connection.Value, message);
            }
        }

        public List<string> GetUsernames()
        {
            return Connections.GetUsernames();
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}