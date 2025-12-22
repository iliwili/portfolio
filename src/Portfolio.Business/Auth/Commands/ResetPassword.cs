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

public class ResetPassword(ResetPasswordRequest request) : ICommand
{
    public ResetPasswordRequest Request { get; set; } = request;
}

public class ResetPasswordHandler(DatabaseContext databaseContext, IDateTimeProvider dateTimeProvider, ILogger<ResetPasswordHandler> logger) : ICommandHandler<ResetPassword>
{
    public async ValueTask<Unit> Handle(ResetPassword command, CancellationToken cancellationToken)
    {
        try
        {
            var now = dateTimeProvider.Now;

            var tokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(command.Request.Token));

            var resetToken = await databaseContext.PasswordResetTokens
                .Include(rt => rt.User)
                .Where(rt => rt.UsedAt == null && rt.ExpiresAt >= now)
                .FirstOrDefaultAsync(rt => rt.Token == tokenHash, cancellationToken);

            if (resetToken == null)
            {
                throw new BusinessException("auth.resetToken.invalid_or_expired");
            }

            resetToken.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Request.NewPassword);
            resetToken.UsedAt = dateTimeProvider.Now;

            await databaseContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during password reset");
            throw new ServerException("auth.resetPassword.failed");
        }
    }
}