using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServerApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ServerApp.Models.DTOs;


[Route("auth")]
[AllowAnonymous] // дозволяє доступ без JWT
public class AuthController : Controller
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult Login() => View();

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login(string username, string password)
    {
        if (username == AdminCredentials.Username && password == AdminCredentials.Password)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSuperSuperSecretKey_123456789!"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // ✅ додаємо роль у claims (як в ApiLogin)
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var tokenOptions = new JwtSecurityToken(
                issuer: "ServerApp",
                audience: "ServerAppUsers",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            Response.Cookies.Append("jwt-token", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return RedirectToAction("Index", "Admin");
        }

        ViewBag.Error = "Invalid username or password";
        return View();
    }

    [HttpPost("api-login")]
    [AllowAnonymous]
    public IActionResult ApiLogin([FromBody] LoginDto login)
    {
        if (login.Username == AdminCredentials.Username && login.Password == AdminCredentials.Password)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSuperSuperSecretKey_123456789!"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // ✅ додаємо роль Admin у claims
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, login.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var tokenOptions = new JwtSecurityToken(
                issuer: "ServerApp",
                audience: "ServerAppUsers",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new { token = tokenString });
        }

        return Unauthorized("Invalid credentials");
    }


}
