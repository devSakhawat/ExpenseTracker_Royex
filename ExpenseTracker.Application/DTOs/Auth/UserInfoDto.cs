namespace ExpenseTracker.Application.DTOs.Auth;

public class UserInfoDto
{
  public int UserId { get; set; }
  public string LoginId { get; set; } = string.Empty;
  public string UserName { get; set; } = string.Empty;
  public string? Email { get; set; }
  public DateTime? LastLoginDate { get; set; }
}
