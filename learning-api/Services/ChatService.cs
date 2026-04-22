using learning_api.Dto;
using learning_api.Repositories;

namespace learning_api.Services
{
    public class ChatService : IChatService
    {
        public readonly IChatRepositories _chatRepositories;
        public readonly UserConnectionManager _userConnectionManager;
        public ChatService(IChatRepositories chatRepositories , UserConnectionManager userConnectionManager) { _chatRepositories = chatRepositories; _userConnectionManager = userConnectionManager; }
        public async Task<object> GetChatUsers(int UserId)
        {
            var chatList = await _chatRepositories.GetChatUsers(UserId);

            foreach (var user in chatList)
            {
                user.IsOnline = _userConnectionManager.IsUserOnline(user.Id);
            }

            return chatList;
        }

        public async Task<object> GetMessages(int UserId, int OtherUserId)
        {
            var messageList = await _chatRepositories.GetMessages(UserId, OtherUserId);

            return messageList.Select(m => new MessagesDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                Text = m.Message,

                Timestamp = m.Timestamp,

                IsSent = m.SenderId == UserId
            }).ToList();
        }
    }
}
