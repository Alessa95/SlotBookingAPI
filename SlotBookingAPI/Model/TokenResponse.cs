﻿namespace SlotBookingAPI.Model
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
