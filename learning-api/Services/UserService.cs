using learning_api.Dto;
using learning_api.Mappers;
using learning_api.Models;
using learning_api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace learning_api.Services
{
    public class UserService :  IUserService
    {
        public readonly IUserRepositories _userRepositories;
        public UserService(IUserRepositories userRepositories) { _userRepositories = userRepositories; }
        public async Task<object> AddUser(UserDto user)
        {
            User existingUser = await _userRepositories.GetUserByEmail(user.Email);

            if (existingUser != null)
            {
                throw new ArgumentException("Email Already Exists");
            }

            //creat a new user object
            User newUser = user.ToEntity();

            await _userRepositories.AddUser(newUser);

            return newUser.ToDto();
        }

        public async Task<object> LogIn(UserDto user)
        {
            User existingUser = await _userRepositories.GetUserByEmail(user.Email);

            if (existingUser == null || existingUser.Password != user.Password)
            {
                throw new UnauthorizedAccessException("Invalid UserName or Password");
            }

            return existingUser.ToDto();
        }

        public async Task<object> Validate(string Email)
        {
            User user = await _userRepositories.GetUserByEmail(Email);

            if (user == null) throw new UnauthorizedAccessException("Unauthorized User");

            return user.ToDto();
        }

        public async Task<object> GetAllUser(string Email)
        {

            List<User> users = await _userRepositories.GetAllUser(Email);

            return users.Select(u => u.ToDto()).ToList();
        }

        public async Task<object> GetUserByPage(string Email, int PageNumber, int PageSize)
        {
            List<User> users = await _userRepositories.GetUserByPage(Email, PageNumber, PageSize);

            var count = await _userRepositories.GetCount(Email);

            return new
            {
                users = users.Select(u => u.ToDto()),
                totalCount = count,
                currentCount = users.Count
            };
        }

        public async Task<object> GetUserById(int Id)
        {
            User user = await _userRepositories.GetUserById(Id);

            if(user == null)
            {
                throw new ArgumentException("User not Found in Id "+Id);
            }

            return user.ToDto();
        }

        public async Task<object> UpdateUser(int Id, UpdateUserDto UpdateUser)
        {
            // find the user with the id
            User user = await _userRepositories.GetUserById(Id);

            // return not found with the error message if user not found
            if (user == null)
            {
                throw new ArgumentException("User not Found in Id " + Id);
            }

            // check the email only after user change the email
            if (user.Email != UpdateUser.Email)
            {
                // check the user already present in the db then return the BadRequest with message
                User existingUser = await _userRepositories.GetUserByEmail(UpdateUser.Email);

                if (existingUser != null)
                {
                    throw new ArgumentException("Email Already Exists");
                }

            }

            // update the record
            user.Name = UpdateUser.Name;
            user.Email = UpdateUser.Email;
            user.PhoneNumber = UpdateUser.PhoneNumber;
            user.Role = UpdateUser.Role;

            // save the chages in the DB
            await _userRepositories.SaveChanges();

            return user.ToDto();
        }

        public async Task RemoveUser(int Id)
        {
            // check the user already present in the db then return the BadRequest with message
            User user = await _userRepositories.GetUserById(Id);

            if (user == null) throw new ArgumentException("User not Found in Id " + Id);

            // remove the record
            await _userRepositories.RemoveUser(user);

            // save the changes in the DB
            await _userRepositories.SaveChanges();
        }

        public async Task<object> Search(string Text, string Email, int PageNumber, int PageSize)
        {
            string searchText = Text.ToLower();
            
            var searchedUsers = await _userRepositories.GetUserBySearch(searchText, Email, PageNumber, PageSize);

            var searchCount = await _userRepositories.GetSearchCount(searchText,Email);

            return new 
            {
                users = searchedUsers,
                totalCount = searchCount,
                currentCount = searchedUsers.Count()
            };
        }

    }
}
