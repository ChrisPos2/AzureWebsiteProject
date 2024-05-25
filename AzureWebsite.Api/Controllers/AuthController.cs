using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging; // Dodaj tę linię

namespace AzureWebsite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings jwtSettings;
        private readonly ILogger<AuthController> logger; // Dodaj tę linię

        public AuthController(JwtSettings jwtSettings, ILogger<AuthController> logger) // Dodaj logger jako zależność
        {
            this.jwtSettings = jwtSettings;
            this.logger = logger; // Przypisz logger
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model.Username == "test" && model.Password == "password") // Replace with your user validation logic
            {
                try
                {
                    var token = GenerateToken(model.Username);
                    return Ok(new { token });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error generating token"); // Zaloguj wyjątek
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            return Unauthorized();
        }

        private string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.ExpiryInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
