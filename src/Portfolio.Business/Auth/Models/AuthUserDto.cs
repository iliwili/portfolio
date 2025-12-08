namespace Portfolio.Business.Auth.Models;

public class AuthUserDto
{
    public string PublicId { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsEmailConfirmed { get; set; }
    public bool IsSuperAdmin { get; set; }
    public List<AccountMembershipDto> Accounts { get; set; } = new();
}

public class AccountMembershipDto
{
    public string PublicId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Role { get; set; } = default!;
    public bool IsOwner { get; set; }
}