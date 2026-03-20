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


        // API - Add new user
        [Route("addUser")]
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> SignUp([FromBody] UserDto user)
        {

            // check the user already present in the db then return the bad request with message
            User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if(existingUser != null)
            {
                return BadRequest(new { message = "Email Already Exists" });
            }

            //creat a new user object
            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };
            
            //add and save the new in DB
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            //return response with the new user
            return Ok( new { Name=newUser.Name, Email = newUser.Email, PhoneNumber = newUser.PhoneNumber , Role = newUser.Role });
        }

        // API - login
        [Route("logIn")]
        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] UserDto user)
        {
            // check the user already present in the db then return the Unauthorized with message
            User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null || existingUser.Password != user.Password)
            {
                return Unauthorized(new { message = "Invalid UserName or Password" });
            }

            // Create a list of claims containing the user's email
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, existingUser.Email)
            };

            // Create a ClaimsIdentity using the claims and cookie authentication
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            // Create a ClaimsPrincipal based on the identity
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user and store authentication info in a cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            //return response with success message
            return Ok(new { message = "Login Successfully." });
        }

        // API - logout
        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user and clear the cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // return response with the success message
            return Ok(new {message = "Logout Successfully"});
        }

        //API - validate
        [Route("validate")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSession()
        {
            // get the current user email fom the cookie
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            // check the user already present in the db then return the Unauthorized with message
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return Unauthorized(new { message = "Unauthorized User" });

            // return the response with the current user details
            return Ok(new {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                role = user.Role
            });
        }

        // API - getAllUser
        [Route("getAllUser")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUser()
        {
            // get the current user email fom the cookie
            var currentEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // Get all the user and admin record without the password except the current user
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

            // return respose with the users
            return Ok(users);
        }

        //API - getUserByPage
        [Route("getUserByPage")]
        [HttpGet]
        public async Task<IActionResult> GetUserByPage([FromQuery] int pageNumber = 1 , [FromQuery] int pageSize = 10)
        {
            // get the current user and admin count
            int currentCount = await _context.Users.CountAsync();

            // get the current user email fom the cookie
            var currentEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // get all the user and admin without the password except the current with the page number and page size
            var listedUsers = await _context.Users.Where(u => u.Email != currentEmail)
                                             .Skip((pageNumber - 1) * pageSize)
                                             .Take(pageSize)
                                             .Select(u => new
                                             {
                                                 id = u.Id,
                                                 name = u.Name,
                                                 email = u.Email,
                                                 phoneNumber = u.PhoneNumber,
                                                 role = u.Role
                                             })
                                             .ToListAsync();
            
            // return the user and admin record, totalcount is currentCount-1 to less the current user count and current finded record size
            return Ok(new
            {
                users = listedUsers,
                totalCount = currentCount-1,
                currentCount = listedUsers.Count()
            });
        } 

        // API - getUserById
        [Route("getUserById/{id}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            // find the user with the id
            User user = await _context.Users.FindAsync(id);

            // return not found with the error message if user not found
            if(user == null)
            {
                return NotFound(new { message = "User Not found in the Given Id:" + id });
            }

            // return the user without password
            return Ok(new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                role = user.Role
            });
        }

        // API - updateUser
        [Route("updateUser/{id}")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUser)
        {
            // find the user with the id
            User user = await _context.Users.FindAsync(id);

            // return not found with the error message if user not found
            if (user == null)
            {
                return NotFound(new { message = "User Not found in the Given Id:" + id });
            }

            // check the email only after user change the email
            if(user.Email != updateUser.Email)
            {
                // check the user already present in the db then return the BadRequest with message
                User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == updateUser.Email);

                if(existingUser != null)
                {
                    return BadRequest(new { message = "Email Already Exists" });
                }

            }

            // update the record
            user.Name = updateUser.Name;
            user.Email = updateUser.Email;
            user.PhoneNumber = updateUser.PhoneNumber;
            user.Role = updateUser.Role;

            // save the chages in the DB
            await _context.SaveChangesAsync();

            // return response with the updated record
            return Ok(new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                role = user.Role
            });
        }

        // API - deleteUserById
        [Route("deleteUserById/{id}")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteUserByID(int id)
        {

            // check the user already present in the db then return the BadRequest with message
            User user = _context.Users.Find(id);

            if (user == null) return NotFound(new { message = "User Not found in the Given Id:" + id });

            // remove the record
            _context.Users.Remove(user);

            // save the changes in the DB
            await _context.SaveChangesAsync();

            // return the response with the success message
            return Ok(new { message = "User Deleted Successfully" });
        }

        // API - search
        [Route("search/{text}")]
        [HttpGet]
        public async Task<IActionResult> Search(string text,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // change the text to lowercase
            text = text.ToLower();

            // get the current email from the cookie
            var currentEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // Query to find the record
            var query =  _context.Users.Where(user => 
                user.Email != currentEmail && (
                user.Id.ToString().Contains(text) ||
                user.Name.ToLower().Contains(text) ||
                user.Email.ToLower().Contains(text) ||
                user.PhoneNumber.ToLower().Contains(text))
            );

            // Count the how many reocrd match the serach text
            var currentCount = await query.CountAsync();

            // get the record that matches the serach text with the "query"
            var listedUsers = await query.Skip((pageNumber - 1) * pageSize)
                                             .Take(pageSize)
                                             .Select(u => new
                                             {
                                                 id = u.Id,
                                                 name = u.Name,
                                                 email = u.Email,
                                                 phoneNumber = u.PhoneNumber,
                                                 role = u.Role
                                             })
                                             .ToListAsync();

            // return the user and admin record, totalCount how many matches the search text in whole DB and currentCount how many details currently matched
            return Ok(new
            {
                users = listedUsers,
                totalCount = currentCount,
                currentCount = listedUsers.Count()
            });

        }

    }
}
