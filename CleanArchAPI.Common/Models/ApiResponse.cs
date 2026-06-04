namespace CleanArchAPI.Common.Models;

/// <summary>
/// Standard API response wrapper used by ALL endpoints.
/// Every response has StatusCode, Success, Message, Data, Pagination.
/// </summary>
public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public PaginationMeta? Pagination { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Success", PaginationMeta? pagination = null)
        => new() { StatusCode = 200, Success = true, Message = message, Data = data, Pagination = pagination };

    public static ApiResponse<T> Created(T data, string message = "Created successfully")
        => new() { StatusCode = 201, Success = true, Message = message, Data = data };

    public static ApiResponse<T> Fail(int statusCode, string message)
        => new() { StatusCode = statusCode, Success = false, Message = message };

    public static ApiResponse<T> NotFound(string message = "Resource not found")
        => new() { StatusCode = 404, Success = false, Message = message };

    public static ApiResponse<T> BadRequest(string message)
        => new() { StatusCode = 400, Success = false, Message = message };

    public static ApiResponse<T> Unauthorized(string message = "Unauthorized")
        => new() { StatusCode = 401, Success = false, Message = message };
}

/// <summary>
/// Pagination info returned in all list responses.
/// </summary>
public class PaginationMeta
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}
