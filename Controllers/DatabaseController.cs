using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DatabaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("check")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                // Attempt to open a connection to the database
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                // If we get here, the connection was successful
                return Ok(new { 
                    status = "Online", 
                    message = "Successfully connected to the 'ecommerce' database.", 
                    serverVersion = connection.ServerVersion 
                });
            }
            catch (Exception ex)
            {
                // If the database is off or credentials are wrong, this returns 500
                return StatusCode(500, new { 
                    status = "Offline", 
                    error = ex.Message 
                });
            }
        }
    }
}