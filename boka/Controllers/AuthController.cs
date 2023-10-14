using boka.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace boka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtAuthService _jwtAuthService;
        public AuthController(IJwtAuthService jwtAuthService, IConfiguration configuration)
        { 
            _configuration = configuration;
            _jwtAuthService = jwtAuthService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]LoginModel model)
        {
            var user = _jwtAuthService.Auth(model.Email, model.Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                };
                var token = _jwtAuthService.TokenGenerer(_configuration["Jwt:Key"], claims);
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}
