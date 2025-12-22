using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Portfolio.Dal.Utils;

public interface ICurrentUser
{
    public string PublicId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
}

public class CurrentUser : ICurrentUser
{
    public string PublicId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            PublicId = user.FindFirst("PublicId")?.Value ?? string.Empty;
            Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            UserName = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }
    }
}