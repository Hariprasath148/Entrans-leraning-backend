using learning_api.Data;
using learning_api.Filters;
using learning_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace learning_api.Controllers
{
    [Route("[Controller]")]
    [ServerException]
    public class ExpectionController : Controller
    {
        public readonly AppDbContext _context;
        public ExpectionController(AppDbContext context) { _context = context; }

        [Route("test")]
        [HttpGet]
        public IActionResult Test()
        {
            User user = _context.Users.Find("janhhari@gmail.com");
            return Ok(user);
        }

    }
}
