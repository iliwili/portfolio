using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Api.Models.Auth;
using Portfolio.Business.Utils;
using Portfolio.Dal;
using Portfolio.Utils;

namespace Portfolio.Business.Auth.Commands;

public class ResetPassword(ResetPasswordRequest request) : ICommand<ApiResponse>
{
    public ResetPasswordRequest Request { get; set; } = request;
}

public class ResetPasswordHandler(
    DatabaseContext databaseContext,
    IDateTimeProvider dateTimeProvider,
    ILogger<ResetPasswordHandler> logger) : ICommandHandler<ResetPassword, ApiResponse>
{
    public async ValueTask<ApiResponse> Handle(ResetPassword command, CancellationToken cancellationToken)
    {
        try
        {
            var resetToken = await databaseContext.PasswordResetTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == command.Request.Token, cancellationToken);

            if (resetToken == null)
            {
                return ApiResponseFactory.BadRequest("Invalid or expired reset token");
            }

            if (resetToken.UsedAt != null)
            {
                return ApiResponseFactory.BadRequest("This reset token has already been used");
            }

            if (resetToken.ExpiresAt < dateTimeProvider.Now)
            {
                return ApiResponseFactory.BadRequest("This reset token has expired");
            }

            // Update password
            resetToken.User.PasswordHash = HashPassword(command.Request.NewPassword);
            resetToken.UsedAt = dateTimeProvider.Now;

            await databaseContext.SaveChangesAsync(cancellationToken);

            return ApiResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during password reset");
            return ApiResponseFactory.Error("An error occurred while resetting your password");
        }
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}

