using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.Auth;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
}

