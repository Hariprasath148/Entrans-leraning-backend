using learning_api.Data;
using learning_api.Dto;
using learning_api.Filters;
using learning_api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace learning_api.Controllers
{
    [Route("[Controller]")]
    //[ServerException]
    public class UserController : Controller
    {
        public readonly AppDbContext _context;
        public UserController(AppDbContext context) { _context = context; }

        [Route("addUser")]
        [HttpPost]
        [Authorize]
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
            Console.WriteLine("hi from the Login gaurd");
            User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null || existingUser.Password != user.Password)
            {
                return Unauthorized(new { message = "Invalid UserName or Password" });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, existingUser.Email)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );
            return Ok(new { message = "Login Successfully." });
        }

        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new {message = "Logout Successfully"});
        }

        [Route("validate")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSession()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return Unauthorized(new { message = "Unauthorized User" });

            return Ok(new {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                role = user.Role
            });
        }

        [Route("getAllUser")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUser()
        {
            var currentEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var users = await _context.Users
                .Where(u => u.Email != currentEmail)
                .Select(u => new
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUser)
        {
            User user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User Not found in the Given Id:" + id });
            }

            if(user.Email != updateUser.Email)
            {
                User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == updateUser.Email);

                if(existingUser != null)
                {
                    return BadRequest(new { message = "Email Already Exists" });
                }

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
        [Authorize]
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
