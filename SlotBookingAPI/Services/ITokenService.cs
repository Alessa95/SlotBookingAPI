using SlotBookingAPI.Model.Authentication;

namespace SlotBookingAPI.Services
{
    public interface ITokenService
    {
        TokenResponse? GenerateJwtToken(TokenRequest tokenRequest);
    }
}