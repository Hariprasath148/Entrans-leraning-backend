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
    }
}
