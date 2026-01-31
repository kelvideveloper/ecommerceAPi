    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using MySqlConnector;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace EcommerceApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly IConfiguration _configuration;

            public AuthController(IConfiguration configuration)
            {
                _configuration = configuration;
            }
// Replace the token generation part in AuthController.cs
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    var user = await GetUserFromDb(request.Email, request.Password);
    if (user == null) return Unauthorized("Invalid credentials");

    // Use short names that match what JsonWebTokenHandler expects
    var claims = new Dictionary<string, object>
    {
        [JwtRegisteredClaimNames.Sub] = user.Email,
        [JwtRegisteredClaimNames.Email] = user.Email,
        ["role"] = user.Role // Use "role" specifically
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Issuer = _configuration["JwtSettings:Issuer"],
        Audience = _configuration["JwtSettings:Audience"],
        Claims = claims,
        Expires = DateTime.UtcNow.AddMinutes(30),
        SigningCredentials = creds
    };

    string token = handler.CreateToken(tokenDescriptor);
    return Ok(new { token });
}

            // Helper method to fetch user (using raw MySqlConnector)
            private async Task<User?> GetUserFromDb(string email, string password)
            {
                using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                await connection.OpenAsync();

                var command = new MySqlCommand("SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new User
                    {
                        Email = reader.GetString("Email"),
                        Role = reader.GetString("Role")
                    };
                }
                return null;
            }
        }

        public class LoginRequest { public string Email { get; set; } public string Password { get; set; } }
        public class User { public string Email { get; set; } public string Role { get; set; } }
    }