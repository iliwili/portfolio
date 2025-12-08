using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Api.Models.Auth;
using Portfolio.Business.Utils;
using Portfolio.Dal;
using Portfolio.Utils;

namespace Portfolio.Business.Auth.Commands;

public class VerifyEmail(VerifyEmailRequest request) : ICommand<ApiResponse>
{
    public VerifyEmailRequest Request { get; set; } = request;
}

public class VerifyEmailHandler(
    DatabaseContext databaseContext,
    IDateTimeProvider dateTimeProvider,
    ILogger<VerifyEmailHandler> logger) : ICommandHandler<VerifyEmail, ApiResponse>
{
    public async ValueTask<ApiResponse> Handle(VerifyEmail command, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: Implement email verification token storage
            // For now, we'll just mark the user as verified based on token lookup
            // In production, you'd store verification tokens in a table similar to password reset tokens

            logger.LogInformation("Email verification attempted with token: {Token}", command.Request.Token);

            // Placeholder implementation - will need EmailVerificationToken entity
            return ApiResponseFactory.BadRequest("Email verification not fully implemented yet");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during email verification");
            return ApiResponseFactory.Error("An error occurred while verifying your email");
        }
    }
}
