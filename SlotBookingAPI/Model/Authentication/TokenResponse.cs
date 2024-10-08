namespace SlotBookingAPI.Model.Authentication
{
    public class TokenResponse
    {
        public required string AccessToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
