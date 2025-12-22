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

public class ForgotPassword(ForgotPasswordRequest request) : ICommand
{
    public ForgotPasswordRequest Request { get; set; } = request;
}

public class ForgotPasswordHandler(
    DatabaseContext databaseContext,
    IDateTimeProvider dateTimeProvider,
    IAuthService authService,
    IOptions<UrlOptions> urlOptions,
    IEmailService emailService,
    ISecureTokenGenerator secureTokenGenerator,
    ILogger<ForgotPasswordHandler> logger) : ICommandHandler<ForgotPassword>
{
    public async ValueTask<Unit> Handle(ForgotPassword command, CancellationToken cancellationToken)
    {
        try
        {
            var user = await databaseContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == command.Request.Email.ToLower(), cancellationToken);

            if (user == null)
            {
                logger.LogWarning("Password reset attempted for non-existent email: {Email}", command.Request.Email);
                return Unit.Value;
            }

            var token = secureTokenGenerator.Generate();
            var now = dateTimeProvider.Now;

            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = secureTokenGenerator.Hash(token),
                CreatedAt = now,
                ExpiresAt = now.AddMinutes(30)
            };

            databaseContext.PasswordResetTokens.Add(resetToken);

            var parameters = new Dictionary<string, object>
            {
                { "reset_password_link", $"{urlOptions.Value.Frontend}/auth/reset-password?token={token}" }
            };
            var mailRequest = new MailRequest
            {
                To = [new Recipient { Email = user.Email, Name = $"{user.FirstName} {user.LastName}" }],
                TemplateId = MailTemplates.ResetPassword,
                Params = parameters
            };
            var messageId = await emailService.SendEmailAsync(mailRequest, cancellationToken);
            resetToken.MessageId = messageId;

            await databaseContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Password reset email sent for user: {Email}", user.Email);

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