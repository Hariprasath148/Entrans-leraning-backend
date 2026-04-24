namespace learning_api.Services
{
    public interface IChatService
    {
        Task<object> GetChatUsers(int UserId);
        Task<object> GetChatUsersWithSearch(int UserId,string SearchText);
        Task<object> GetMessages(int UserId, int OtherUserId);
    }
}
