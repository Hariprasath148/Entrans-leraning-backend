using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace learning_api.Filters
{
    public class ServerExceptionAttribute :  ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new ContentResult
            {
                Content = "Internal Server Error",
                StatusCode = 500
            };

            context.ExceptionHandled = true;

        }
    }
}
