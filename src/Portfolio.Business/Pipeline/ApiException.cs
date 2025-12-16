using Microsoft.AspNetCore.Http;

namespace Portfolio.Business.Pipeline;

public abstract class ApiException(string errorCode, object? errorArgs = null, Exception? inner = null) : Exception(errorCode, inner)
{
    public string ErrorCode { get; } = errorCode;
    public object? ErrorArgs { get; } = errorArgs;

    /// <summary>Optional: if set, the error should be shown under a specific field.</summary>
    public string? Field { get; init; }

    /// <summary>Optional: override HTTP status.</summary>
    public virtual int StatusCode { get; } = StatusCodes.Status400BadRequest;
}

public sealed class NotFoundException(string errorCode = "common.not_found", object? args = null)
    : ApiException(errorCode, args)
{
    public override int StatusCode => StatusCodes.Status404NotFound;
}

public sealed class UnauthorizedException(string errorCode = "auth.unauthorized", object? args = null)
    : ApiException(errorCode, args)
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;
}

public sealed class ForbiddenException(string errorCode = "auth.forbidden", object? args = null)
    : ApiException(errorCode, args)
{
    public override int StatusCode => StatusCodes.Status403Forbidden;
}

/// <summary>For business rules that should show on a particular form field.</summary>
public sealed class FieldException : ApiException
{
    public override int StatusCode => StatusCodes.Status400BadRequest;

    public FieldException(string field, string errorCode, object? args = null)
        : base(errorCode, args)
    {
        Field = field;
    }
}

/// <summary>For generic server-side issues you want to map cleanly.</summary>
public sealed class ServerException(string errorCode = "common.error", object? args = null, Exception? inner = null)
    : ApiException(errorCode, args, inner)
{
    public override int StatusCode => StatusCodes.Status500InternalServerError;
}