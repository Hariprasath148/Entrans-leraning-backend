namespace learning_api.Services
{
    public interface IChatService
    {
        Task<object> GetChatUsers(int UserId);
        Task<object> GetMessages(int UserId, int OtherUserId);
    }
}
