using Ecom.API.Errors;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware>logger,IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                _logger.LogInformation("Success");
            }
            catch(Exception ex)
            {
                _logger.LogError($"this error comes from Middleware {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response= _env.IsDevelopment()?
                new ApiException<object>((int)HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace.ToString())
                : new ApiException<object>((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json);
              
            }
        }
    }
}
