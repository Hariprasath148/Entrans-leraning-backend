using learning_api.Data;
using learning_api.Dto;
using learning_api.Filters;
using learning_api.Models;
using learning_api.Services;
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
    public class UserController : ControllerBase
    {
        public readonly AppDbContext _context;
        public readonly IUserService _userService;
        public UserController(AppDbContext context , IUserService userService) { _context = context; _userService = userService; }

        // API - Add new user
        [Route("addUser")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SignUp([FromBody] UserDto user)
        {
            // check the user already present in the db then return the bad request with message
            var newUser = await _userService.AddUser(user);
            //return response with the new user
            return Ok(newUser);
        }

        // API - login
        [Route("logIn")]
        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] UserDto user)
        {

            UserReturnDto existingUser = (UserReturnDto) await _userService.LogIn(user);
            
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
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "Invalid or expired session" });
            }
            // get the current user email fom the cookie
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            //pass the email to the service
            var user = await _userService.Validate(email);

            return Ok(user);
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
            var users = await _userService.GetAllUser(currentEmail);

            // return respose with the users
            return Ok(users);
        }


        //API - getUserByPage
        [Route("getUserByPage")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserByPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // get the current user email fom the cookie
            var currentEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var users = await _userService.GetUserByPage(currentEmail, pageNumber, pageSize);

            return Ok(users);
        }

        // API - getUserById
        [Route("getUserById/{id}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);

            return Ok(user);
        }

        // API - updateUser
        [Route("updateUser/{id}")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUser)
        {
            var user = await _userService.UpdateUser(id, updateUser);

            return Ok(user);
        }

        // API - deleteUserById
        [Route("deleteUserById/{id}")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteUserByID(int id)
        {

            await _userService.RemoveUser(id);

            // return the response with the success message
            return Ok(new { message = "User Deleted Successfully" });
        }

        // API - search
        [Route("search/{text}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search(string text,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            // get the current email from the cookie
            var currentEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var users = await _userService.Search(text, currentEmail, pageNumber, pageSize);

            // return the user and admin record, totalCount how many matches the search text in whole DB and currentCount how many details currently matched
            return Ok(users);
        }

    }
}
