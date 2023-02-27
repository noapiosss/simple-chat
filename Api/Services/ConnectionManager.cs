using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Api.Services
{
    public class ConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _connections = new();

        public bool UsernameIsAlreadyInUse(string username)
        {
            return _connections.ContainsKey(username);
        }

        public void AddSocket(string username, WebSocket socket)
        {
            _connections.TryAdd(username, socket);
        }

        public async Task RemoveSocketAsync(string username)
        {
            _connections.TryRemove(username, out WebSocket socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket connection closed", CancellationToken.None);
        }

        public WebSocket GetSocketByUsername(string username)
        {
            return _connections.FirstOrDefault(c => c.Key == username).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAllConnections()
        {
            return _connections;
        }

        public string GetUsername(WebSocket socket)
        {
            return _connections.FirstOrDefault(c => c.Value == socket).Key;
        }
    }
}