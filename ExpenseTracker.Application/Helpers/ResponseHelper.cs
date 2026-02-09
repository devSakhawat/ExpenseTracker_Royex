using ExpenseTracker.Application.DTOs.Common;

namespace ExpenseTracker.Application.Helpers;

public static class ResponseHelper
{
  public static ApiResponse<T> Success<T>(T data, string? message = null)
    => new(data, 200, message ?? "Operation completed successfully");

  public static ApiResponse<T> Created<T>(T data, string? message = null)
    => new(data, 201, message ?? "Resource created successfully");

  public static ApiResponse<T> NoContent<T>(string? message = null)
    => new(204, message ?? "No content available");

  public static ApiResponse BadRequest(string? message = null)
    => new(400, message ?? "Bad request") { ErrorType = "BadRequest" };

  public static ApiResponse Unauthorized(string? message = null)
    => new(401, message ?? "Unauthorized") { ErrorType = "Unauthorized" };

  public static ApiResponse NotFound(string? message = null)
    => new(404, message ?? "Not found") { ErrorType = "NotFound" };

  public static ApiResponse Conflict(string? message = null)
    => new(409, message ?? "Conflict") { ErrorType = "Conflict" };

  public static ApiResponse InternalServerError(string? message = null)
    => new(500, message ?? "Internal server error") { ErrorType = "InternalServerError" };
}
