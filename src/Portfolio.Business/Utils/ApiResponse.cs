namespace Portfolio.Business.Utils;

public class ApiResponse(bool success, string? errorCode, string? errorMessage)
{
    public bool Success { get; set; } = success;
    public string? ErrorCode { get; set; } = errorCode;
    public string? ErrorMessage { get; set; } = errorMessage;
}

public class ApiResponse<T>(bool success, string? errorCode, string? errorMessage, T data) : ApiResponse(success, errorCode, errorMessage)
{
    public T Data { get; set; } = data;
}