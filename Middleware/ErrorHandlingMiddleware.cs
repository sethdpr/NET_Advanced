using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NET_Advanced.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_next == null)
            {
                throw new InvalidOperationException("RequestDelegate _next is null. Controleer of de middleware correct is geregistreerd.");
            }

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string message = $"Er is een fout opgetreden: {ex.Message}";
                _logger?.LogError(ex, message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("Er is iets mis gegaan. Probeer het later opnieuw.");
            }
        }


    }
}
