using Microsoft.AspNetCore.Http;
using Portfolio.Business.Pipeline;

namespace Portfolio.Business.Errors;

public class BusinessException(
    string errorCode,
    object? args = null,
    int statusCode = StatusCodes.Status400BadRequest
) : ApiException(errorCode, args)
{
    public override int StatusCode { get; } = statusCode;
}