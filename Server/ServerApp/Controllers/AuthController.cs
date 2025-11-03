using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSuperSuperSecretKey_123456789!"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
    issuer: "ServerApp",
    audience: "ServerAppUsers",
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
