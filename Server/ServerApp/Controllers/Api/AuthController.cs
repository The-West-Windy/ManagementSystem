using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using ServerApp.Models;
using ServerApp.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServerApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var librarian = await _context.Librarians.FirstOrDefaultAsync(l => l.Email == request.Email);
            if (librarian == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, librarian.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            var jwtSection = _configuration.GetSection("Jwt");
            var secret = jwtSection["Key"] ?? throw new InvalidOperationException("JWT secret key is not configured");
            var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured");
            var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("JWT audience is not configured");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, librarian.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, librarian.Email),
                new Claim(ClaimTypes.Name, librarian.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = tokenString });
        }
    }
}
