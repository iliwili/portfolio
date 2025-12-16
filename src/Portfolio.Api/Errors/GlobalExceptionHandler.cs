using System.Text.Json;
using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;
using Portfolio.Business.Errors;
using Portfolio.Business.Pipeline;

namespace Portfolio.Api.Errors;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (problem, statusCode) = exception switch
        {
            ApiValidationException vex => (BuildValidationProblem(httpContext, vex.Failures), StatusCodes.Status400BadRequest),
            ApiException aex => (BuildApiProblem(httpContext, aex), aex.StatusCode),
            _ => (BuildUnhandledProblem(httpContext, exception), StatusCodes.Status500InternalServerError),
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problem, JsonOptions, cancellationToken: cancellationToken);
        return true;
    }

    private static object BuildApiProblem(HttpContext ctx, ApiException ex)
    {
        // Handle field-specific errors
        if (!string.IsNullOrWhiteSpace(ex.Field))
        {
            var field = FieldPath.Normalize(ex.Field!);

            var fieldResponse = new Dictionary<string, object>
            {
                ["type"] = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                ["title"] = "Request could not be processed.",
                ["status"] = ex.StatusCode,
                ["instance"] = ctx.Request.Path.Value ?? "",
                ["errors"] = new Dictionary<string, string[]>
                {
                    [field] = [ex.ErrorCode]
                },
                ["traceId"] = ctx.TraceIdentifier
            };

            // Optional: args for the field
            if (ex.ErrorArgs is not null)
            {
                fieldResponse["errorArgsByField"] = new Dictionary<string, object?>
                {
                    [field] = ex.ErrorArgs
                };
            }

            return fieldResponse;
        }

        // Handle general API errors
        var generalResponse = new Dictionary<string, object>
        {
            ["type"] = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            ["title"] = "Request could not be processed.",
            ["status"] = ex.StatusCode,
            ["instance"] = ctx.Request.Path.Value ?? "",
            ["errorCode"] = ex.ErrorCode,
            ["traceId"] = ctx.TraceIdentifier
        };

        if (ex.ErrorArgs is not null)
            generalResponse["errorArgs"] = ex.ErrorArgs;

        return generalResponse;
    }

    private static object BuildValidationProblem(HttpContext ctx, IReadOnlyList<ValidationFailure> failures)
    {
        var errors = failures
            .GroupBy(f => FieldPath.Normalize(f.PropertyName))
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => string.IsNullOrWhiteSpace(f.ErrorCode) ? "validation.invalid" : f.ErrorCode).ToArray()
            );

        // Optional: carry args (CustomState) per field, per error
        var argsByField = failures
            .GroupBy(f => FieldPath.Normalize(f.PropertyName))
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.CustomState).ToArray()
            );

        var response = new Dictionary<string, object>
        {
            ["type"] = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            ["title"] = "One or more validation errors occurred.",
            ["status"] = StatusCodes.Status400BadRequest,
            ["instance"] = ctx.Request.Path.Value ?? "",
            ["errors"] = errors,
            ["errorArgsByField"] = argsByField,
            ["traceId"] = ctx.TraceIdentifier
        };

        return response;
    }

    private object BuildUnhandledProblem(HttpContext ctx, Exception ex)
    {
        logger.LogError(ex, "Unhandled exception");

        return new Dictionary<string, object>
        {
            ["type"] = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            ["title"] = "An unexpected error occurred.",
            ["status"] = StatusCodes.Status500InternalServerError,
            ["instance"] = ctx.Request.Path.Value ?? "",
            ["errorCode"] = "common.error",
            ["traceId"] = ctx.TraceIdentifier
        };
    }
}