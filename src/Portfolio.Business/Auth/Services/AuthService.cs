using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Dal;
using Portfolio.Dal.Entities;

namespace Portfolio.Business.Auth.Services;

public interface IAuthService
{
    Task SignInAsync(User user);
    string GenerateSecureToken();

    Task<string> GenerateSlug(string name, CancellationToken cancellationToken = default);
}

public class AuthService(ILogger<AuthService> logger, IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext) : IAuthService
{
    public async Task SignInAsync(User user)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            logger.LogWarning("HttpContext is null, unable to sign in user");
            return;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new("PublicId", user.PublicId),
        };

        if (user.IsSuperAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "SuperAdmin"));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            // ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        };

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    public string GenerateSecureToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public async Task<string> GenerateSlug(string name, CancellationToken cancellationToken)
    {
        // Simple slug generation - lowercase, replace spaces with hyphens
        var slug = name.ToLower()
            .Replace(" ", "-")
            .Replace("_", "-");

        // Remove special characters
        slug = new string(slug.Where(c => char.IsLetterOrDigit(c) || c == '-').ToArray());

        // Ensure uniqueness
        var baseSlug = slug;
        var counter = 1;

        while (await databaseContext.Accounts.AnyAsync(a => a.Slug == slug, cancellationToken))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }
}