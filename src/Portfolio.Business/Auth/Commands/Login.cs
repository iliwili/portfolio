using Mediator;
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

public class Login(LoginRequest loginRequest) : ICommand<ApiResponse<AuthUserDto>>
{
    public LoginRequest Request { get; set; } = loginRequest;
}

public class LoginHandler(DatabaseContext databaseContext, ILogger<LoginHandler> logger, IDateTimeProvider dateTimeProvider, IAuthService authService) : ICommandHandler<Login, ApiResponse<AuthUserDto>>
{
    public async ValueTask<ApiResponse<AuthUserDto>> Handle(Login command, CancellationToken cancellationToken)
    {
        try
        {
            var user = await databaseContext.Users
                .Include(u => u.AccountUsers)
                .ThenInclude(au => au.Account)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == command.Request.Email.ToLower(), cancellationToken);

            if (user == null)
            {
                return ApiResponseFactory.BadRequest<AuthUserDto>("Invalid email or password");
            }

            if (!VerifyPassword(command.Request.Password, user.PasswordHash))
            {
                return ApiResponseFactory.Error<AuthUserDto>("Invalid email or password");
            }

            // Update last login
            user.LastLoginAt = dateTimeProvider.Now;
            await databaseContext.SaveChangesAsync(cancellationToken);

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
            logger.LogError(ex, "Error during login");
            return ApiResponseFactory.Error<AuthUserDto>("An error occurred during login");
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