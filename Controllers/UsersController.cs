using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // Only Admins can access this
        [HttpGet("admin-data")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            return Ok("Welcome Admin! You have access to sensitive data.");
        }

        // Only Common users can access this
        [HttpGet("common-data")]
        [Authorize(Roles = "Common")]
        public IActionResult GetCommonData()
        {
            return Ok("Welcome User! This is your standard dashboard.");
        }
        
        // Both can access this (Authorized but any role)
        [HttpGet("public-data")]
        [Authorize]
        public IActionResult GetPublicData()
        {
            return Ok("If you see this, you are logged in (Role doesn't matter).");
        }
    }
}