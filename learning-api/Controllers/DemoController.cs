using Microsoft.AspNetCore.Mvc;

namespace learning_api.Controllers
{
    [Route("/hello")]
    public class DemoController : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "hari Prasath";
            return View();
        }

        [Route("hello/{id}")]
        public IActionResult GetId(int id)
        {
            return Ok(new { Id = id});
        }
        [Route("{id}/{name}/{age}")] 
        public IActionResult GetDetails(int id,string name,int age)
        {
            return Ok(new { id , name , age});
        }

    }
}
