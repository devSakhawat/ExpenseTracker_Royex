using System.Net;
using System.Text.Json;
using ExpenseTracker.Application.DTOs.Common;
using ExpenseTracker.Domain.Exceptions;

namespace ExpenseTracker.Api.Middleware;

public class ExceptionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionMiddleware> _logger;

  public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
      await HandleExceptionAsync(context, ex);
    }
  }

  private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    context.Response.ContentType = "application/json";

    var (statusCode, message) = exception switch
    {
      BadRequestException => ((int)HttpStatusCode.BadRequest, exception.Message),
      NotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
      UnauthorizedException => ((int)HttpStatusCode.Unauthorized, exception.Message),
      ConflictException => ((int)HttpStatusCode.Conflict, exception.Message),
      _ => ((int)HttpStatusCode.InternalServerError, "An internal server error occurred.")
    };

    context.Response.StatusCode = statusCode;

    var response = new ApiResponse(statusCode, message)
    {
      ErrorType = exception.GetType().Name
    };

    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
  }
}
