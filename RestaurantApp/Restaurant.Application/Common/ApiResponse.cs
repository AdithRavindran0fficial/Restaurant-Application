namespace Restaurant.Application.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public List<string>? Errors { get; set; }

    private ApiResponse(bool success, string message, T? data, int statusCode, List<string>? errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        StatusCode = statusCode;
        Errors = errors;
    }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>(true, message, data, statusCode);
    }

    public static ApiResponse<T> CreatedResponse(T data, string message = "Created successfully", int statusCode = 201)
    {
        return new ApiResponse<T>(true, message, data, statusCode);
    }

    public static ApiResponse<T> FailureResponse(string message, int statusCode = 400, List<string>? errors = null)
    {
        return new ApiResponse<T>(false, message, default, statusCode, errors);
    }

    public static ApiResponse<T> NotFoundResponse(string message = "Resource not found")
    {
        return new ApiResponse<T>(false, message, default, 404);
    }

    public static ApiResponse<T> UnauthorizedResponse(string message = "Unauthorized")
    {
        return new ApiResponse<T>(false, message, default, 401);
    }

    public static ApiResponse<T> ForbiddenResponse(string message = "Forbidden")
    {
        return new ApiResponse<T>(false, message, default, 403);
    }

    public static ApiResponse<T> ConflictResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>(false, message, default, 409, errors);
    }

    public static ApiResponse<T> ValidationErrorResponse(string message = "Validation failed", List<string>? errors = null)
    {
        return new ApiResponse<T>(false, message, default, 422, errors);
    }

    public static ApiResponse<T> ServerErrorResponse(string message = "Internal server error")
    {
        return new ApiResponse<T>(false, message, default, 500);
    }
}

public static class ApiResponse
{
    public static ApiResponse<object> SuccessResponse(string message = "Success", int statusCode = 200)
    {
        return ApiResponse<object>.SuccessResponse(null, message, statusCode);
    }

    public static ApiResponse<object> CreatedResponse(string message = "Created successfully", int statusCode = 201)
    {
        return ApiResponse<object>.CreatedResponse(null, message, statusCode);
    }

    public static ApiResponse<object> FailureResponse(string message, int statusCode = 400, List<string>? errors = null)
    {
        return ApiResponse<object>.FailureResponse(message, statusCode, errors);
    }

    public static ApiResponse<object> NotFoundResponse(string message = "Resource not found")
    {
        return ApiResponse<object>.NotFoundResponse(message);
    }

    public static ApiResponse<object> UnauthorizedResponse(string message = "Unauthorized")
    {
        return ApiResponse<object>.UnauthorizedResponse(message);
    }

    public static ApiResponse<object> ForbiddenResponse(string message = "Forbidden")
    {
        return ApiResponse<object>.ForbiddenResponse(message);
    }

    public static ApiResponse<object> ConflictResponse(string message, List<string>? errors = null)
    {
        return ApiResponse<object>.ConflictResponse(message, errors);
    }

    public static ApiResponse<object> ValidationErrorResponse(string message = "Validation failed", List<string>? errors = null)
    {
        return ApiResponse<object>.ValidationErrorResponse(message, errors);
    }

    public static ApiResponse<object> ServerErrorResponse(string message = "Internal server error")
    {
        return ApiResponse<object>.ServerErrorResponse(message);
    }
}
