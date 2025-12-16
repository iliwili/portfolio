using FluentValidation.Results;

namespace Portfolio.Business.Errors;

public class ApiValidationException : Exception
{
    public IReadOnlyList<ValidationFailure> Failures { get; }

    public ApiValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation errors occurred.")
    {
        Failures = failures.ToList();
    }
}