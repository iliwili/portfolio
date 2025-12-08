using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Business.Auth.Models;
using Portfolio.Business.Utils;
using Portfolio.Dal;
using Portfolio.Dal.Entities;
using Portfolio.Dal.Utils;

namespace Portfolio.Business.Auth.Queries;

public class GetMe : IQuery<ApiResponse<AuthUserDto>>
{
}

public class GetMeHandler(DatabaseContext databaseContext, ILogger<GetMeHandler> logger, ICurrentUser currentUser) : IQueryHandler<GetMe, ApiResponse<AuthUserDto>>
{
    public async ValueTask<ApiResponse<AuthUserDto>> Handle(GetMe query, CancellationToken cancellationToken)
    {
        try
        {
            var user = await databaseContext.Users
                .Include(u => u.AccountUsers)
                .ThenInclude(au => au.Account)
                .FirstOrDefaultAsync(u => u.PublicId == currentUser.PublicId, cancellationToken);


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
            logger.LogError(ex, "Error fetching user with ID {UserId}", currentUser.PublicId);
            return ApiResponseFactory.Error<AuthUserDto>("An error occurred while fetching your profile");
        }
    }
}