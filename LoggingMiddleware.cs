using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UserManagementAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request details
            var requestPath = context.Request.Path.Value;
            var method = context.Request.Method;

            // Log the request
            Console.WriteLine($"Incoming request: {method} {requestPath}");

            await _next(context);

            // Log response details
            var statusCode = context.Response.StatusCode;

            // Log the response
            Console.WriteLine($"Outgoing response: {statusCode}");
        }
    }
}