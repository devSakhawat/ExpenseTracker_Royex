namespace ExpenseTracker.Domain.Entities;

public class User
{
  public int UserId { get; set; }
  public string LoginId { get; set; } = string.Empty;
  public string UserName { get; set; } = string.Empty;
  public string PasswordHash { get; set; } = string.Empty;
  public string? Email { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedDate { get; set; }
  public DateTime? LastLoginDate { get; set; }

  // Navigation
  public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
