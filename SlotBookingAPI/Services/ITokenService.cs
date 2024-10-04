namespace SlotBookingAPI.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(string userId);
    }
}