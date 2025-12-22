using System.Security.Cryptography;
using System.Text;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Business.Auth.Models;
using Portfolio.Business.Errors;
using Portfolio.Dal;
using Portfolio.Utils;

namespace Portfolio.Business.Auth.Commands;

public class VerifyEmail(VerifyEmailRequest request) : ICommand
{
    public VerifyEmailRequest Request { get; set; } = request;
}

public class VerifyEmailHandler(
    DatabaseContext databaseContext,
    IDateTimeProvider dateTimeProvider,
    ILogger<VerifyEmailHandler> logger) : ICommandHandler<VerifyEmail>
{
    public async ValueTask<Unit> Handle(VerifyEmail command, CancellationToken cancellationToken)
    {
        try
        {
            var now = dateTimeProvider.Now;

            var tokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(command.Request.Token));

            var verificationToken = await databaseContext.EmailVerificationTokens
                .Include(x => x.User)
                .Where(rt => rt.UsedAt == null && rt.ExpiresAt >= now)
                .FirstOrDefaultAsync(x => x.Token == tokenHash, cancellationToken);

            if (verificationToken == null)
            {
                throw new BusinessException("auth.emailVerification.invalid_or_expired");
            }

            verificationToken.User.IsEmailConfirmed = true;
            verificationToken.UsedAt = dateTimeProvider.Now;

            await databaseContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Email verified successfully for user: {Email}", verificationToken.User.Email);
            return Unit.Value;
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during email verification");
            throw new ServerException("auth.emailVerification.failed");
        }
    }
}