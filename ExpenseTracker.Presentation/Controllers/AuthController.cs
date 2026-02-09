using System.Security.Claims;
using ExpenseTracker.Application.DTOs.Auth;
using ExpenseTracker.Application.Helpers;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Presentation.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Presentation.Controllers;

[ApiController]
[Route(RouteConstants.BaseRoute + "/auth")]
public class AuthController : ControllerBase
{
  private readonly IServiceManager _serviceManager;

  public AuthController(IServiceManager serviceManager)
  {
    _serviceManager = serviceManager;
  }

  [HttpPost(RouteConstants.Register)]
  public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
  {
    var result = await _serviceManager.Authentication.RegisterAsync(dto);
    return Ok(ResponseHelper.Created(result, "User registered successfully."));
  }

  [HttpPost(RouteConstants.Login)]
  public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
  {
    var result = await _serviceManager.Authentication.LoginAsync(dto);
    return Ok(ResponseHelper.Success(result, "Login successful."));
  }

  [HttpPost(RouteConstants.RefreshToken)]
  public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
  {
    var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
    var result = await _serviceManager.Authentication.RefreshTokenAsync(dto.RefreshToken, ipAddress);
    return Ok(ResponseHelper.Success(result, "Token refreshed successfully."));
  }

  [HttpPost(RouteConstants.RevokeToken)]
  [Authorize]
  public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequestDto dto)
  {
    var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
    var result = await _serviceManager.Authentication.RevokeTokenAsync(dto.RefreshToken, ipAddress);
    if (!result)
      return NotFound(ResponseHelper.NotFound("Token not found or already revoked."));
    return Ok(ResponseHelper.Success(result, "Token revoked successfully."));
  }

  [HttpGet(RouteConstants.UserInfo)]
  [Authorize]
  public async Task<IActionResult> GetUserInfo()
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
      return Unauthorized(ResponseHelper.Unauthorized("Invalid token."));

    var result = await _serviceManager.Authentication.GetUserInfoAsync(userId);
    return Ok(ResponseHelper.Success(result, "User info retrieved successfully."));
  }
}
