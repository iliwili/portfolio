using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Api.Models.Auth;
using Portfolio.Business.Auth.Services;
using Portfolio.Business.Pipeline;
using Portfolio.Dal;
using Portfolio.Dal.Entities;
using Portfolio.Utils;

namespace Portfolio.Business.Auth.Commands;

public class ForgotPassword(ForgotPasswordRequest request) : ICommand
{
    public ForgotPasswordRequest Request { get; set; } = request;
}

public class ForgotPasswordHandler(DatabaseContext databaseContext, ILogger<ForgotPasswordHandler> logger, IDateTimeProvider dateTimeProvider, IAuthService authService) : ICommandHandler<ForgotPassword>
{
    public async ValueTask<Unit> Handle(ForgotPassword command, CancellationToken cancellationToken)
    {
        try
        {
            var user = await databaseContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == command.Request.Email.ToLower(), cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("auth.user_not_found");
            }

            var token = authService.GenerateSecureToken();
            var now = dateTimeProvider.Now;

            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                CreatedAt = now,
                ExpiresAt = now.AddHours(24)
            };

            databaseContext.PasswordResetTokens.Add(resetToken);
            await databaseContext.SaveChangesAsync(cancellationToken);

            // TODO: Send password reset email
            logger.LogInformation("Password reset token for {Email}: {Token}", command.Request.Email, token);

            return Unit.Value;
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during forgot password");
            throw new ServerException("auth.forgotPassword.failed");
        }
    }
}