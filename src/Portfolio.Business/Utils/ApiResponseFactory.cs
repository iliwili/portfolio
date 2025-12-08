namespace Portfolio.Business.Utils;

public static class ApiResponseFactory
{
    public static ApiResponse Ok() => new(true, null, null);
    public static ApiResponse<T> Ok<T>(T data)
        => new(true, null, null, data);

    public static ApiResponse<T> Created<T>(T data)
        => new(true, null, null, data);

    public static ApiResponse NotFound(string? message = null)
        => new(false, "not_found", message ?? "Resource not found.");
    public static ApiResponse<T> NotFound<T>(string? message = null)
        => new(false, "not_found", message ?? "Resource not found.", default);

    public static ApiResponse BadRequest(string message, string? code = "bad_request")
        => new(false, code, message);
    public static ApiResponse<T> BadRequest<T>(string message, string? code = "bad_request")
        => new(false, code, message, default);

    public static ApiResponse Error(string message, string? code = "error")
        => new(false, code, message);
    public static ApiResponse<T> Error<T>(string message, string? code = "error")
        => new(false, code, message, default);

    public static ApiResponse<T> Unauthorized<T>(string message = "Unauthorized.")
        => new(false, "unauthorized", message, default);

    public static ApiResponse<T> Forbidden<T>(string message = "Forbidden.")
        => new(false, "forbidden", message, default);
}