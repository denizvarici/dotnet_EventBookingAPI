using EventBooking.API.Models;
using EventBooking.Application.Common;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace EventBooking.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var response = new Result<object>
            {
                Success = false,
                Message = exception is ValidationException ? "Validasyon hatası!" : exception.Message,
                Errors = exception switch
                {
                    ValidationException vex => vex.Errors.Select(e => e.ErrorMessage),
                    _ => new List<string> { exception.Message }
                }
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
