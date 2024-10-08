namespace SlotBookingAPI.Model.Authentication
{
    public class TokenRequest
    {
        public required string User { get; set; }
        public required string Password { get; set; }
    }
}