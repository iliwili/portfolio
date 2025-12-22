using FluentValidation;
using Portfolio.Business.Auth.Models;

namespace Portfolio.Business.Auth.Validators;

public class RegisterValidator : FluentValidation.AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("validation.required")
            .EmailAddress().WithErrorCode("validation.email");

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode("validation.required")
            .MinimumLength(8)
            .WithErrorCode("validation.minLength")
            .WithState(_ => new { min = 8 });
    }
}