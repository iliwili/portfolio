using Mediator;
using Microsoft.Extensions.Logging;
using Portfolio.Api.Models.Auth;
using Portfolio.Business.Errors;
using Portfolio.Business.Pipeline;

namespace Portfolio.Business.Auth.Commands;

public class VerifyEmail(VerifyEmailRequest request) : ICommand
{
    public VerifyEmailRequest Request { get; set; } = request;
}

public class VerifyEmailHandler(ILogger<VerifyEmailHandler> logger) : ICommandHandler<VerifyEmail>
{
    public async ValueTask<Unit> Handle(VerifyEmail command, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: Implement email verification token storage
            // For now, we'll just mark the user as verified based on token lookup
            // In production, you'd store verification tokens in a table similar to password reset tokens

            logger.LogInformation("Email verification attempted with token: {Token}", command.Request.Token);

            // Placeholder implementation - will need EmailVerificationToken entity
            throw new BusinessException("Email verification not fully implemented yet");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during email verification");
            throw new ServerException("An error occurred while verifying your email");
        }
    }
}