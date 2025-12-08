using System.Security.Claims;
using System.Security.Cryptography;
using Mediator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Api.Models.Auth;
using Portfolio.Business.Auth.Models;
using Portfolio.Business.Auth.Services;
using Portfolio.Business.Utils;
using Portfolio.Dal;
using Portfolio.Dal.Entities;
using Portfolio.Utils;

namespace Portfolio.Business.Auth.Commands;

public class Register(RegisterRequest registerRequest) : ICommand<ApiResponse<AuthUserDto>>
{
    public RegisterRequest Request { get; set; } = registerRequest;
}

public class RegisterCommandHandler(
    DatabaseContext databaseContext,
    IDateTimeProvider dateTimeProvider,
    IAuthService authService,
    ILogger<RegisterCommandHandler> logger) : ICommandHandler<Register, ApiResponse<AuthUserDto>>
{
    public async ValueTask<ApiResponse<AuthUserDto>> Handle(Register command, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user already exists
            var existingUser = await databaseContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == command.Request.Email.ToLower(), cancellationToken);

            if (existingUser != null)
            {
                return ApiResponseFactory.BadRequest<AuthUserDto>("A user with this email already exists");
            }

            // Check if username is taken
            var existingUsername = await databaseContext.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == command.Request.UserName.ToLower(), cancellationToken);

            if (existingUsername != null)
            {
                return ApiResponseFactory.BadRequest<AuthUserDto>("This username is already taken");
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
            var slug = await GenerateSlugAsync(command.Request.AccountName, cancellationToken);
            var account = new Account
            {
                PublicId = GeneratePublicId(),
                Name = command.Request.AccountName,
                Slug = slug,
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

            return ApiResponseFactory.Ok(new AuthUserDto
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
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during user registration");
            return ApiResponseFactory.Error<AuthUserDto>("An error occurred during registration");
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

    private async Task<string> GenerateSlugAsync(string name, CancellationToken cancellationToken)
    {
        // Simple slug generation - lowercase, replace spaces with hyphens
        var slug = name.ToLower()
            .Replace(" ", "-")
            .Replace("_", "-");

        // Remove special characters
        slug = new string(slug.Where(c => char.IsLetterOrDigit(c) || c == '-').ToArray());

        // Ensure uniqueness
        var baseSlug = slug;
        var counter = 1;

        while (await databaseContext.Accounts.AnyAsync(a => a.Slug == slug, cancellationToken))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }
}