using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.Auth;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}

