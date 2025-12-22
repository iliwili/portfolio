using System.ComponentModel.DataAnnotations;

namespace Portfolio.Business.Auth.Models;

public class ResendVerificationEmailRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
}