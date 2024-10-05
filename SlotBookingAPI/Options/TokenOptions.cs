namespace SlotBookingAPI.Options
{
    public class TokenOptions
    {
        public string Path { get; set; } = "/token";

        public string? Key { get; set; }

        public string? Issuer { get; set; }

        public string? Audience { get; set; }

        public TimeSpan SessionDuration { get; set; }
    }
}
