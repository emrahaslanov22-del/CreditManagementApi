using CreditManagementApi.Dtos;
using CreditManagementApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CreditManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configruation;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configruation = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Fullname = model.Fullname
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new
            {
                message = "User registered successfully"

            });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "User logged in successfully",
                token = token
            });
        }

        private object GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.NameIdentifier,user.Id),

               new Claim(ClaimTypes.Name,user.UserName!),

               new Claim(ClaimTypes.Email, user.Email!)
           };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configruation["Jwt:Key"]));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configruation["Jwt:Issuer"],
                audience: _configruation["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
