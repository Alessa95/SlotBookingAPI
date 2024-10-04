using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SlotBookingAPI.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SlotBookingAPI.Services
{
    public class TokenService(IOptions<TokenOptions> tokenOptions) : ITokenService
    {
        public string GenerateJwtToken(string userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value.Key ?? throw new InvalidOperationException("Config setting is required: Jwt:Key")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: tokenOptions.Value.Issuer,
                audience: tokenOptions.Value.Audience,
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                },
                expires: DateTime.UtcNow.Add(tokenOptions.Value.SessionDuration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
