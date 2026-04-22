using System.Collections.Concurrent;

namespace learning_api.Services
{
    public class UserConnectionManager
    {
        private readonly ConcurrentDictionary<int, string> _connections = new();

        public void AddConnection(int userId, string connectionId)
        {
            _connections[userId] = connectionId;
        }

        public string GetConnection(int userId)
        {
            _connections.TryGetValue(userId, out var connectionId);
            return connectionId;
        }

        public void RemoveConnection(int userId)
        {
            _connections.TryRemove(userId, out _);
        }

        public bool IsUserOnline(int userId)
        {
            return _connections.ContainsKey(userId);
        }
    }
}
