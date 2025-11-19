using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerApp.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("auth")]
[AllowAnonymous] // дозволяє доступ без JWT
public class AuthController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthController(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult Login() => View();

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Please enter both username and password.";
            return View();
        }

        var adminUser = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);
        if (adminUser is null || !BCrypt.Net.BCrypt.Verify(password, adminUser.PasswordHash))
        {
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        var jwtSection = _configuration.GetSection("Jwt");
        var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured");
        var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("JWT audience is not configured");
        var key = jwtSection["Key"] ?? throw new InvalidOperationException("JWT key is not configured");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, adminUser.ID.ToString()),
            new Claim(ClaimTypes.Name, adminUser.Username),
            new Claim(ClaimTypes.Role, adminUser.Role)
        };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        Response.Cookies.Append("jwt-token", tokenString, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Strict
        });

        return RedirectToAction("Index", "Admin");
    }
}