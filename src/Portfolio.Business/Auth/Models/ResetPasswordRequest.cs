using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.Auth;

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; } = default!;

    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = default!;
}