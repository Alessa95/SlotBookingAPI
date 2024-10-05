using SlotBookingAPI.Model;

namespace SlotBookingAPI.Services
{
    public interface ITokenService
    {
        TokenResponse? GenerateJwtToken(TokenRequest tokenRequest);
    }
}