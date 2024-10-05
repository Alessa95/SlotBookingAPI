using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;

namespace SlotBookingAPI.Middleware
{
    public class ErrorHandlerMiddleware(RequestDelegate _next)
    {
        public async Task Invoke(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = MediaTypeNames.Application.Json;

            var statusCode = MapExceptionToStatusCode(exception);

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            response.StatusCode = statusCode;
            return response.WriteAsync(result);
        }

        private int MapExceptionToStatusCode(Exception exception)
        {
            return exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound, 
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, 
                ArgumentException => (int)HttpStatusCode.BadRequest,
                ValidationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
