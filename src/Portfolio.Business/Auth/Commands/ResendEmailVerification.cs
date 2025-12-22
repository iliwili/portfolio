using System.Security.Cryptography;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Business.Auth.Helpers;
using Portfolio.Business.Auth.Models;
using Portfolio.Business.Auth.Services;
using Portfolio.Business.Configuration;
using Portfolio.Business.Emails.Models;
using Portfolio.Business.Emails.Services;
using Portfolio.Business.Emails.Templates;
using Portfolio.Business.Errors;
using Portfolio.Dal;
using Portfolio.Dal.Entities;
using Portfolio.Utils;

namespace Portfolio.Business.Auth.Commands;

public class ResendEmailVerification(ResendVerificationEmailRequest request) : ICommand
{
    public ResendVerificationEmailRequest Request { get; set; } = request;
}

public class ResendEmailVerificationHandler(
    DatabaseContext databaseContext,
    IDateTimeProvider dateTimeProvider,
    IAuthService authService,
    ILogger<ResendEmailVerificationHandler> logger) : ICommandHandler<ResendEmailVerification>
{
    public async ValueTask<Unit> Handle(ResendEmailVerification command, CancellationToken cancellationToken)
    {
        try
        {
            var user = await databaseContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == command.Request.Email.ToLower(), cancellationToken);

            if (user == null)
            {
                logger.LogWarning("Resend verification attempted for non-existent email: {Email}", command.Request.Email);
                return Unit.Value;
            }

            if (user.IsEmailConfirmed)
            {
                throw new BusinessException("auth.email_already_verified");
            }

            // Invalidate any existing unused tokens for this user
            var emailVerificationTokens = await databaseContext.EmailVerificationTokens
                .Where(evt => evt.UserId == user.Id && evt.UsedAt == null)
                .ToListAsync(cancellationToken);
            foreach (var verificationToken in emailVerificationTokens)
            {
                verificationToken.UsedAt = dateTimeProvider.Now;
            }

            await authService.SendVerificationEmailAsync(user, cancellationToken);

            await databaseContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Verification email resent for user: {Email}", user.Email);
            return Unit.Value;
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during resend verification email");
            throw new ServerException("auth.resendVerification.failed");
        }
    }

    private string GenerateSecureToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}