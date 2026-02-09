using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Application.DTOs.Auth;

public class LoginRequestDto
{
  [Required(ErrorMessage = "LoginId is required.")]
  public string LoginId { get; set; } = string.Empty;

  [Required(ErrorMessage = "Password is required.")]
  public string Password { get; set; } = string.Empty;
}
