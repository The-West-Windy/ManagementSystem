using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using ServerApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            var jwtSection = _configuration.GetSection("Jwt");
            var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured");
            var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("JWT audience is not configured");
            var key = jwtSection["Key"] ?? throw new InvalidOperationException("JWT key is not configured");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: new List<Claim>(),
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
}
