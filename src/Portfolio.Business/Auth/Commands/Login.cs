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

public class Login(LoginRequest loginRequest) : ICommand<AuthUserDto>
{
    public LoginRequest Request { get; set; } = loginRequest;
}

public class LoginHandler(DatabaseContext databaseContext, ILogger<LoginHandler> logger, IDateTimeProvider dateTimeProvider, IAuthService authService) : ICommandHandler<Login, AuthUserDto>
{
    public async ValueTask<AuthUserDto> Handle(Login command, CancellationToken cancellationToken)
    {
        try
        {
            var email = command.Request.Email.Trim().ToLower();

            var user = await databaseContext.Users
                .Include(u => u.AccountUsers)
                .ThenInclude(au => au.Account)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email, cancellationToken);

            if (user is null)
            {
                throw new FieldException("email", "auth.email_not_registered");
            }

            if (!VerifyPassword(command.Request.Password, user.PasswordHash))
            {
                throw new FieldException("password", "auth.invalid_credentials");
            }

            // Update last login
            user.LastLoginAt = dateTimeProvider.Now;
            await databaseContext.SaveChangesAsync(cancellationToken);

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
            logger.LogError(ex, "Error during login");
            throw new ServerException("common.error");
        }
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch
        {
            return false;
        }
    }
}