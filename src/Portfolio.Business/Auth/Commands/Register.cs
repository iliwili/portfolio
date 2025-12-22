using System.Security.Cryptography;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

public class Register(RegisterRequest registerRequest) : ICommand<AuthUserDto>
{
    public RegisterRequest Request { get; set; } = registerRequest;
}

public class RegisterCommandHandler(
    DatabaseContext databaseContext,
    IDateTimeProvider dateTimeProvider,
    IAuthService authService,
    IOptions<UrlOptions> urlOptions,
    IEmailService emailService,
    ILogger<RegisterCommandHandler> logger) : ICommandHandler<Register, AuthUserDto>
{
    public async ValueTask<AuthUserDto> Handle(Register command, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await databaseContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == command.Request.Email.ToLower(), cancellationToken);

            if (existingUser != null)
            {
                throw new FieldException("email", "auth.email_already_registered", command.Request.Email);
            }

            var existingUsername = await databaseContext.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == command.Request.UserName.ToLower(), cancellationToken);

            if (existingUsername != null)
            {
                throw new FieldException("username", "auth.username_already_registered", command.Request.UserName);
            }

            var now = dateTimeProvider.Now;

            var user = new User
            {
                FirstName = command.Request.FirstName,
                LastName = command.Request.LastName,
                UserName = command.Request.UserName,
                Email = command.Request.Email.ToLower(),
                PasswordHash = HashPassword(command.Request.Password),
                IsEmailConfirmed = false,
                IsSuperAdmin = false,
                CreatedAt = now
            };

            databaseContext.Users.Add(user);

            var account = new Account
            {
                Name = command.Request.AccountName,
                Slug = command.Request.Slug,
                CreatedAt = now,
                Owner = user
            };

            databaseContext.Accounts.Add(account);

            var accountUser = new AccountUser
            {
                Account = account,
                User = user,
                Role = AccountRole.Owner,
                JoinedAt = now
            };

            databaseContext.AccountUsers.Add(accountUser);

            await authService.SendVerificationEmailAsync(user, cancellationToken);

            await databaseContext.SaveChangesAsync(cancellationToken);

            await databaseContext.Entry(user)
                .Collection(u => u.AccountUsers)
                .Query()
                .Include(au => au.Account)
                .LoadAsync(cancellationToken);

            // Sign in the user
            await authService.SignInAsync(user);

            return new AuthUserDto
            {
                PublicId = user.PublicId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                IsEmailConfirmed = user.IsEmailConfirmed,
                IsSuperAdmin = user.IsSuperAdmin,
                Accounts = user.AccountUsers.Select(x => new AccountMembershipDto
                {
                    PublicId = x.Account.PublicId,
                    Name = x.Account.Name,
                    Slug = x.Account.Slug,
                    Role = x.Role.ToString(),
                    IsOwner = x.Role == AccountRole.Owner
                }).ToList()
            };
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during user registration");
            throw new ServerException("auth.registration.failed");
        }
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private string GeneratePublicId()
    {
        return Guid.NewGuid().ToString("N");
    }

    private string GenerateSecureToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}