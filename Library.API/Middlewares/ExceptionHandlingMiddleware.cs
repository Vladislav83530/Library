using Library.API.Model;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Library.API.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var errorMessage = "An error occurred.";

                _logger.LogError(ex, errorMessage);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var error = new Error
                {
                    Message = errorMessage,
                    Details = ex.Message
                };

                var json = JsonSerializer.Serialize(error);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
