using Azure;
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
                await _next(context);

                if (!context.Response.HasStarted && context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandleNotFoundEndPoint(context);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionsAsync(context, ex);
            }
        }

        private static async Task HandleExceptionsAsync(HttpContext context, Exception ex)
        {
            var res = new ErrorToReturn()
            {
                ErrorMessage = ex.Message
            };
            res.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException badRequestException => GetBadRequestErrors(badRequestException, res),
                _ => StatusCodes.Status500InternalServerError
            };
            
            await context.Response.WriteAsJsonAsync(res);
        }

        private static int GetBadRequestErrors(BadRequestException badRequestException, ErrorToReturn res)
        {
            res.Errors = badRequestException.Errors;
            return StatusCodes.Status400BadRequest;
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
