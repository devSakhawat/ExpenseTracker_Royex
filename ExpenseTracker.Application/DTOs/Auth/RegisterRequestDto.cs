using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Application.DTOs.Auth;

public class RegisterRequestDto
{
  [Required(ErrorMessage = "LoginId is required.")]
  [MaxLength(50)]
  public string LoginId { get; set; } = string.Empty;

  [Required(ErrorMessage = "UserName is required.")]
  [MaxLength(100)]
  public string UserName { get; set; } = string.Empty;

  [Required(ErrorMessage = "Password is required.")]
  [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
  public string Password { get; set; } = string.Empty;

  [MaxLength(200)]
  [EmailAddress(ErrorMessage = "Invalid email format.")]
  public string? Email { get; set; }
}
