namespace ExpenseTracker.Domain.Entities;

public class RefreshToken
{
  public int RefreshTokenId { get; set; }
  public int UserId { get; set; }
  public string Token { get; set; } = string.Empty;
  public DateTime ExpiryDate { get; set; }
  public DateTime CreatedDate { get; set; }
  public string CreatedByIp { get; set; } = string.Empty;
  public bool IsRevoked { get; set; }
  public DateTime? RevokedDate { get; set; }
  public string? ReplacedByToken { get; set; }

  public bool IsActive => !IsRevoked && ExpiryDate > DateTime.UtcNow;

  // Navigation
  public User User { get; set; } = null!;
}
