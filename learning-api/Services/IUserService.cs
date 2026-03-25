using learning_api.Dto;

namespace learning_api.Services
{
    public interface IUserService
    {
        Task<object> AddUser(UserDto user);
        Task<object> LogIn(UserDto user);
        Task<object> Validate(string Email);
        Task<object> GetAllUser(string Email);
        Task<object> GetUserByPage(string Email, int PageNumber, int PageSize);
        Task<object> GetUserById(int Id);
        Task<object> UpdateUser(int Id, UpdateUserDto UpdateUser);
        Task RemoveUser(int Id);
        Task<object> Search(string Text, string Email, int PageNumber, int PageSize);
    }
}
