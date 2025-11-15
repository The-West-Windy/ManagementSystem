using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerApp.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==================== SERVICES ====================

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer(); // для Swagger
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ======== JWT Authentication Setup ========

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSecretKey = jwtSection["Key"] ?? throw new InvalidOperationException("JWT secret key is not configured");
var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured");
var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("JWT audience is not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("jwt-token"))
            {
                context.Token = context.Request.Cookies["jwt-token"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ==================== BUILD ====================

var app = builder.Build();

// ==================== MIDDLEWARE ====================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServerApp API V1");
    c.RoutePrefix = "swagger"; // UI буде доступний за /swagger
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ==================== ROUTES ====================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
