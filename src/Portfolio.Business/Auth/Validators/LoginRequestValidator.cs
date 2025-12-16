using FluentValidation;
using Portfolio.Api.Models.Auth;

namespace Portfolio.Business.Auth.Validators;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("validation.required")
            .EmailAddress().WithErrorCode("validation.email");

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode("validation.required")
            .MinimumLength(8).WithErrorCode("validation.minLength");
    }
}