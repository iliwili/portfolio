using System.Security.Cryptography;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Api.Models.Auth;
using Portfolio.Business.Auth.Models;
using Portfolio.Business.Auth.Services;
using Portfolio.Business.Pipeline;
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
    IValidator<RegisterRequest> validator,
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

            // Create user
            var user = new User
            {
                PublicId = GeneratePublicId(),
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

            // Create account
            var account = new Account
            {
                PublicId = GeneratePublicId(),
                Name = command.Request.AccountName,
                Slug = command.Request.Slug,
                CreatedAt = now,
                Owner = user
            };

            databaseContext.Accounts.Add(account);

            // Create account-user relationship
            var accountUser = new AccountUser
            {
                PublicId = GeneratePublicId(),
                Account = account,
                User = user,
                Role = AccountRole.Owner,
                JoinedAt = now
            };

            databaseContext.AccountUsers.Add(accountUser);

            // Create email verification token
            var verificationToken = GenerateSecureToken();
            // TODO: Store verification token and send email
            logger.LogInformation("Email verification token for {Email}: {Token}", command.Request.Email, verificationToken);

            await databaseContext.SaveChangesAsync(cancellationToken);

            // Load relationships for DTO
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