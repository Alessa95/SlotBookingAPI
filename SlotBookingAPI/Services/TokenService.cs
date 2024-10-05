using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SlotBookingAPI.Model.Authentication;
using SlotBookingAPI.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SlotBookingAPI.Services
{
    public class TokenService(IOptions<TokenOptions> tokenOptions, IAuthService authService) : ITokenService
    {
        public TokenResponse? GenerateJwtToken(TokenRequest tokenRequest)
        {
            
            var identity = authService.ValidateUser(tokenRequest.User, tokenRequest.Password);
            if (!identity)
            {
               return null;
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value.Key ?? throw new InvalidOperationException("Config setting is required: Jwt:Key")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: tokenOptions.Value.Issuer,
                audience: tokenOptions.Value.Audience,
                claims: GetClaims(tokenRequest.User),
                expires: DateTime.UtcNow.Add(tokenOptions.Value.SessionDuration),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponse
            {
                AccessToken = encodedToken,
                ExpiresIn = (int)tokenOptions.Value.SessionDuration.TotalSeconds
            };
        }

        private IEnumerable<Claim> GetClaims(string username)
        {
            return
            [
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Email, $"{username}@slotBooking.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];
        }
    }
}
