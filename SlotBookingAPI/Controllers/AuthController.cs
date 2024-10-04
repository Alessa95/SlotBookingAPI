using Microsoft.AspNetCore.Mvc;
using SlotBookingAPI.Model;
using SlotBookingAPI.Services;

namespace SlotBookingAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(ITokenService tokenService, IAuthService authService) : ControllerBase
    {
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (authService.ValidateUser(model.User, model.Password))
            {
                var token = tokenService.GenerateJwtToken(model.User);
                return Ok(new { token });
            }
            return Unauthorized();
        }
    }
}
