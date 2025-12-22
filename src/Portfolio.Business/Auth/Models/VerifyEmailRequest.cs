using System.ComponentModel.DataAnnotations;

namespace Portfolio.Business.Auth.Models;

public class VerifyEmailRequest
{
    [Required]
    public string Token { get; set; } = default!;
}

