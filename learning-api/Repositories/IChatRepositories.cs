using learning_api.Dto;
using learning_api.Models;

namespace learning_api.Repositories
{
    public interface IChatRepositories
    {
        Task<List<ChatListDto>> GetChatUsers(int UserId);
        Task<List<ChatMessage>> GetMessages(int UserId, int OtherUserId);
        Task AddMessage(ChatMessage ChatMessage);
    }
}
