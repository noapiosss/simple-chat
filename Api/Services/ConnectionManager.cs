using System.Collections.Concurrent;
using System.Net.WebSockets;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class ConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _connections = new();
        private readonly IUserHandler _userHandler;

        public ConnectionManager(IUserHandler userHandler)
        {
            _userHandler = userHandler;
        }

        public void AddSocket(string username, WebSocket socket)
        {
            _userHandler.TryAddUser(username);
            _connections.TryAdd(username, socket);            
        }

        public async Task RemoveSocketAsync(string username)
        {            
            _userHandler.TryRemoveUser(username);
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

        public List<string> GetUsernames()
        {
            return _connections.Keys.ToList();
        }

        public string GetUsername(WebSocket socket)
        {
            return _connections.FirstOrDefault(c => c.Value == socket).Key;
        }
    }
}