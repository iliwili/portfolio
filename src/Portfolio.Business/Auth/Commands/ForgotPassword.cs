using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Api.Models.Auth;
using Portfolio.Business.Auth.Services;
using Portfolio.Business.Utils;
using Portfolio.Dal;
using Portfolio.Dal.Entities;
using Portfolio.Utils;

namespace Portfolio.Business.Auth.Commands;

public class ForgotPassword(ForgotPasswordRequest request) : ICommand<ApiResponse>
{
    public ForgotPasswordRequest Request { get; set; } = request;
}

public class ForgotPasswordHandler(DatabaseContext databaseContext, ILogger<ForgotPasswordHandler> logger, IDateTimeProvider dateTimeProvider, IAuthService authService) : ICommandHandler<ForgotPassword, ApiResponse>
{
    public async ValueTask<ApiResponse> Handle(ForgotPassword command, CancellationToken cancellationToken)
    {
        try
        {
            var user = await databaseContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == command.Request.Email.ToLower(), cancellationToken);

            if (user == null)
            {
                // Don't reveal if user exists
                return ApiResponseFactory.NotFound("No user with this email was found");
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

            return ApiResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during forgot password");
            return ApiResponseFactory.Error("An error occurred while processing your request");
        }
    }
}