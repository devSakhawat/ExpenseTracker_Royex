namespace ExpenseTracker.Application.DTOs.Auth;

public class TokenResponseDto
{
  public string AccessToken { get; set; } = string.Empty;
  public string RefreshToken { get; set; } = string.Empty;
  public DateTime AccessTokenExpiry { get; set; }
  public DateTime RefreshTokenExpiry { get; set; }
  public string TokenType { get; set; } = "Bearer";
  public int ExpiresIn { get; set; }
}
