using Microsoft.AspNetCore.Mvc.RazorPages;

namespace learning_api.Pages
{
    public class Index : PageModel
    {
        public string Message { get; set; }
        public void OnGet()
        {
            Message = "Hari Prasath";
        }
    }
}
