namespace learning_api.Services
{
    public interface IChatService
    {
        Task<object> GetChatUsers(int UserId);
    }
}
