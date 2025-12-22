using FluentValidation;
using Portfolio.Business.Auth.Models;

namespace Portfolio.Business.Auth.Validators;

public sealed class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("validation.required")
            .EmailAddress().WithErrorCode("validation.email");

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode("validation.required")
            .MinimumLength(8).WithErrorCode("validation.minLength");
    }
}