using System.Text;

namespace Library.API.Middlewares
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            finally
            {
                var request = context.Request;
                var response = context.Response;

                var requestBody = await GetRequestBodyAsync(request);

                var message = BuildLogMessage(request, response, requestBody);

                _logger.LogInformation(message);

            }
        }

        private async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private string BuildLogMessage(HttpRequest request, HttpResponse response, string requestBody)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[{DateTime.Now}]");
            sb.AppendLine($"{request.Method} {request.Path}{request.QueryString}");
            sb.AppendLine($"Request body: {requestBody}");
            sb.AppendLine($"Status: {response.StatusCode}");
            sb.AppendLine("Headers: ");
            foreach (string key in request.Headers.Keys)
            {
                sb.AppendLine("\t" + key + ": " + request.Headers[key]);
            }

            return sb.ToString();
        }
    }
}