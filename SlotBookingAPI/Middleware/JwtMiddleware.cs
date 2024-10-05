using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SlotBookingAPI.Services;
using TokenOptions = SlotBookingAPI.Options.TokenOptions;

namespace SlotBookingAPI.Middleware
{
    public class JwtMiddleware(RequestDelegate next, IOptions<TokenOptions> tokenOptions, ITokenService tokenService)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (!IsTokenRequest(context))
            {
                await next(context);
                return;
            }

            if (!IsValidRequest(context))
            {
                await WriteErrorResponse(context, 400, "Bad request. Only POST requests with JSON content are allowed.");
                return;
            }

            var formData = await context.Request.ReadFormAsync();
            var tokenRequest = ParseTokenRequest(formData);

            if (tokenRequest == null)
            {
                await WriteErrorResponse(context, 400, "Invalid request data. Username and password are required.");
                return;
            }

            var token = tokenService.GenerateJwtToken(tokenRequest);

            if (token == null)
            {
                await WriteErrorResponse(context, 400, "Invalid username or password.");
                return;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(token, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private bool IsTokenRequest(HttpContext context)
        {
            return context.Request.Path.Equals(tokenOptions.Value.Path, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsValidRequest(HttpContext context)
        {
            return context.Request.Method.Equals("POST") && context.Request.HasJsonContentType();
        }

        private Model.TokenRequest? ParseTokenRequest(IFormCollection formData)
        {
            var username = formData["username"].ToString();
            var password = formData["password"].ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return new Model.TokenRequest
            {
                User = username,
                Password = password
            };
        }

        private Task WriteErrorResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var errorResponse = new { message };
            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
    }
}
