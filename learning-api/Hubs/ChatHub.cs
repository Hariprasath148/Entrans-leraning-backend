using learning_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace learning_api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserConnectionManager _connectionManager;

        public ChatHub(UserConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        // Register user when connected
        public async Task RegisterUser(int userId)
        {
            _connectionManager.AddConnection(userId, Context.ConnectionId);
            await Task.CompletedTask;
        }

        // Send private message
        public async Task SendPrivateMessage(int senderId, int receiverId, string message)
        {
            var receiverConnectionId = _connectionManager.GetConnection(receiverId);

            if (receiverConnectionId != null)
            {
                await Clients.Client(receiverConnectionId)
                    .SendAsync("ReceivePrivateMessage", senderId, message);
            }
        }
    }
}
