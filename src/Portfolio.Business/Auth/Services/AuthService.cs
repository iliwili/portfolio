using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Business.Auth.Helpers;
using Portfolio.Business.Configuration;
using Portfolio.Business.Emails.Models;
using Portfolio.Business.Emails.Services;
using Portfolio.Business.Emails.Templates;
using Portfolio.Dal;
using Portfolio.Dal.Entities;
using Portfolio.Utils;

namespace Portfolio.Business.Auth.Services;

public interface IAuthService
{
    Task SignInAsync(User user);
    Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken = default);
    Task<string> GenerateSlug(string name, CancellationToken cancellationToken = default);
}

public class AuthService(
    DatabaseContext databaseContext,
    IDateTimeProvider dateTimeProvider,
    ISecureTokenGenerator secureTokenGenerator,
    IHttpContextAccessor httpContextAccessor,
    IOptions<UrlOptions> urlOptions,
    IEmailService emailService,
    ILogger<AuthService> logger) : IAuthService
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
    public async Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken = default)
    {
        var now = dateTimeProvider.Now;
        var token = secureTokenGenerator.Generate();

        var emailVerificationToken = new EmailVerificationToken
        {
            User = user,
            Token = secureTokenGenerator.Hash(token),
            ExpiresAt = now.AddHours(24)
        };

        databaseContext.EmailVerificationTokens.Add(emailVerificationToken);

        // Send verification email
        var parameters = new Dictionary<string, object>
        {
            { "verification_link", $"{urlOptions.Value.Frontend}/auth/verify-email?token={token}" }
        };
        var mailRequest = new MailRequest
        {
            To = [new Recipient { Email = user.Email, Name = $"{user.FirstName} {user.LastName}" }],
            TemplateId = MailTemplates.ConfirmEmail,
            Params = parameters
        };

        var messageId = await emailService.SendEmailAsync(mailRequest, cancellationToken);
        emailVerificationToken.MessageId = messageId;
    }


    public async Task<string> GenerateSlug(string name, CancellationToken cancellationToken)
    {
        var slug = name.ToLower()
            .Replace(" ", "-")
            .Replace("_", "-");

        slug = new string(slug.Where(c => char.IsLetterOrDigit(c) || c == '-').ToArray());

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