using DomainLayer.Exceptions;
using Shared;
using System.Text.Json;

namespace E_CommerceAPI.CustomMiddleWares
{
    public class CustomExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

        public CustomExceptionHandlerMiddleWare(RequestDelegate Next,ILogger<CustomExceptionHandlerMiddleWare> logger) 
        {
            _next=Next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                await HandleNotFoundEndPoint(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionsAsync(context, ex);
            }
        }

        private static async Task HandleExceptionsAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            context.Response.ContentType = "application/json";
            var res = new ErrorToReturn()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = ex.Message
            };
            await context.Response.WriteAsJsonAsync(res);
        }

        private static async Task HandleNotFoundEndPoint(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var res = new ErrorToReturn()
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorMessage = $"The EndPoint {context.Request.Path} was not found."
                };
                await context.Response.WriteAsJsonAsync(res);
            }
        }
    }
}
