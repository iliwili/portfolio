using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Api.Models.Auth;
using Portfolio.Business.Errors;
using Portfolio.Business.Pipeline;
using Portfolio.Dal;
using Portfolio.Utils;

namespace Portfolio.Business.Auth.Commands;

public class ResetPassword(ResetPasswordRequest request) : ICommand
{
    public ResetPasswordRequest Request { get; set; } = request;
}

public class ResetPasswordHandler(
    DatabaseContext databaseContext,
    IDateTimeProvider dateTimeProvider,
    ILogger<ResetPasswordHandler> logger) : ICommandHandler<ResetPassword>
{
    public async ValueTask<Unit> Handle(ResetPassword command, CancellationToken cancellationToken)
    {
        try
        {
            var resetToken = await databaseContext.PasswordResetTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == command.Request.Token, cancellationToken);

            if (resetToken == null)
            {
                throw new BusinessException("auth.resetToken.invalid_or_expired");
            }

            if (resetToken.UsedAt != null)
            {
                throw new BusinessException("auth.resetToken.invalid_or_expired");
            }

            if (resetToken.ExpiresAt < dateTimeProvider.Now)
            {
                throw new BusinessException("auth.resetToken.invalid_or_expired");
            }

            // Update password
            resetToken.User.PasswordHash = HashPassword(command.Request.NewPassword);
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

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}