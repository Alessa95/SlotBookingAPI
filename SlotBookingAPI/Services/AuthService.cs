using Microsoft.Extensions.Options;
using SlotBookingAPI.Options;

namespace SlotBookingAPI.Services
{
    public class AuthService(IOptions<AvailabilityApiOptions> options) : IAuthService
    {
        public bool ValidateUser(string username, string password)
        {
            return username == options.Value.ApiUsername && password == options.Value.ApiPassword;
        }
    }
}
