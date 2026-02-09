namespace ExpenseTracker.Application.DTOs.Common;

public class ApiResponse<T>
{
  public T? Data { get; set; }
  public int StatusCode { get; set; }
  public string? Message { get; set; }
  public bool IsSuccess { get; set; }

  public ApiResponse(T data, int statusCode, string? message = null)
  {
    Data = data;
    StatusCode = statusCode;
    Message = message;
    IsSuccess = statusCode >= 200 && statusCode < 300;
  }

  public ApiResponse(int statusCode, string? message = null)
  {
    StatusCode = statusCode;
    Message = message;
    IsSuccess = statusCode >= 200 && statusCode < 300;
  }
}

public class ApiResponse
{
  public int StatusCode { get; set; }
  public string? Message { get; set; }
  public bool IsSuccess { get; set; }
  public string? ErrorType { get; set; }
  public Dictionary<string, string[]>? ValidationErrors { get; set; }

  public ApiResponse(int statusCode, string? message = null)
  {
    StatusCode = statusCode;
    Message = message;
    IsSuccess = statusCode >= 200 && statusCode < 300;
  }
}
