using Newtonsoft.Json;
using System.Net;

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
            response.ContentType = "application/json";

            var statusCode = MapExceptionToStatusCode(exception);

            if (exception is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
            }

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
                _ => (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
