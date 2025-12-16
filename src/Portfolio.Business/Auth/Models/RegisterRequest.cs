using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.Auth;

public class RegisterRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = default!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = default!;

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string UserName { get; set; } = default!;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = default!;

    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string AccountName { get; set; } = default!;

    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string Slug { get; set; } = default!;
}