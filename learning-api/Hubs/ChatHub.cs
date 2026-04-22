using learning_api.Models;
using learning_api.Repositories;
using learning_api.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace learning_api.Hubs
{
    public class ChatHub : Hub
    {
        public readonly IChatRepositories _chatRepositories;
        public readonly UserConnectionManager _connectionManager;
        public ChatHub(IChatRepositories chatRepositories, UserConnectionManager userConnectionManager) { _chatRepositories = chatRepositories; _connectionManager = userConnectionManager; }
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

            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await _chatRepositories.AddMessage(chatMessage);
        }
    }
}
