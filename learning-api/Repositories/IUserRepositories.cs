using learning_api.Dto;
using learning_api.Models;

namespace learning_api.Repositories
{
    public interface IUserRepositories
    {
        Task SaveChanges();
        Task<User> GetUserByEmail(string Email);
        Task<User> GetUserById(int Id);
        Task AddUser(User user);
        Task<List<User>> GetAllUser(string Email);
        Task<List<User>> GetUserByPage(string Email,int PageNumber,int PageSize=10);
        Task<int> GetCount(string Email);
        Task RemoveUser(User user);
        Task<int> GetSearchCount(string Text, string Email);
        Task<List<User>> GetUserBySearch(string Text, string Email,int PageNumber, int PageSize=10);
    }
}