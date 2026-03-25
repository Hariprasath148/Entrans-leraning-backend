using System.Net;
using System.Text.Json;

namespace learning_api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next) { _next = next; }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                await HandleException(context,e);
            }
        }
        public static async Task HandleException(HttpContext context, Exception e)
        {
            context.Response.ContentType = "/application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Something wen wrong";
            
            if(e is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = e.Message;
            }

            if(e is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = e.Message;
            }

            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new { message = message });

            await context.Response.WriteAsync(result);
        }
    }
}
