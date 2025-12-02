using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Portfolio.Dal;
using Portfolio.Utils;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("Default");
    options.UseNpgsql(cs);
});

// Register HttpContextAccessor (needed for permission checks)
builder.Services.AddHttpContextAccessor();

// Register utilities
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

// CORS (Nuxt dashboard)
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dashboard", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "https://app.yourdomain.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Cookie auth
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "portfolio_auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument((options, _) =>
{
    options.Title = "Portfolio Api";
    options.Version = "v1";
    options.DocumentName = "v1";
});


builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

app.UseCors("Dashboard");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseOpenApi(options => { options.Path = "/openapi/{documentName}.json"; });
app.MapScalarApiReference();

app.MapControllers();

app.Run();