using learning_api.Dto;
using learning_api.Models;

namespace learning_api.Mappers
{
    public static class UserMapper
    {
        public static object ToDto(this User user)
        {
            return new UserReturnDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };
        }

        public static User ToEntity(this UserDto user)
        {
            return new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };
        }
    }
}
