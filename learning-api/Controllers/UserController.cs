using learning_api.Data;
using learning_api.Dto;
using learning_api.Filters;
using learning_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace learning_api.Controllers
{
    [Route("[Controller]")]
    [ServerException]
    public class UserController : Controller
    {
        public readonly AppDbContext _context;
        public UserController(AppDbContext context) { _context = context; }

        [Route("addUser")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] UserDto user)
        {

            User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if(existingUser != null)
            {
                return BadRequest(new { message = "Email Already Exists" });
            }

            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok( new { Name=newUser.Name, Email = newUser.Email, PhoneNumber = newUser.PhoneNumber , Role = newUser.Role });
        }

        [Route("logIn")]
        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] UserDto user)
        {
            User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null || existingUser.Password != user.Password)
            {
                return NotFound(new { message = "Invalid UserName or Password" });
            }

            return Ok(new { message = "Login Successfully." });
        }

        [Route("getAllUser")]
        [HttpGet] 
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _context.Users.Select(u => new
            {
                id = u.Id,
                name = u.Name,
                email = u.Email,
                phoneNumber = u.PhoneNumber,
                role = u.Role
            }).ToListAsync();

            return Ok(users);
        }

        [Route("getUserById/{id}")]
        [HttpGet] 
        public async Task<IActionResult> GetUserById(int id)
        {
            User user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                return NotFound(new { message = "User Not found in the Given Id:" + id });
            }

            return Ok(new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                role = user.Role
            });
        }

        [Route("updateUser/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUser)
        {
            User user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User Not found in the Given Id:" + id });
            }

            User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == updateUser.Email);

            if (existingUser != null && user.Email != updateUser.Email )
            {
                return BadRequest(new { message = "Email Already Exists" });
            }

            user.Name = updateUser.Name;
            user.Email = updateUser.Email;
            user.PhoneNumber = updateUser.PhoneNumber;
            user.Role = updateUser.Role;

            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [Route("deleteUserById/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserByID(int id)
        {

            User user = _context.Users.Find(id);

            if (user == null) return NotFound(new { message = "User Not found in the Given Id:" + id });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User Deleted Successfully" });
        }
    }
}
