using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.Auth;

public class VerifyEmailRequest
{
    [Required]
    public string Token { get; set; } = default!;
}

