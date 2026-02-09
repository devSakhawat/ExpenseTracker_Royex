using ExpenseTracker.Application.DTOs.Auth;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface IAuthenticationService
{
  Task<string> RegisterAsync(RegisterRequestDto dto);
  Task<TokenResponseDto> LoginAsync(LoginRequestDto dto);
  Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress);
  Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress);
  Task RemoveExpiredTokensAsync();
  Task RevokeAllUserTokensAsync(int userId, string ipAddress);
  Task<UserInfoDto> GetUserInfoAsync(int userId);
}
