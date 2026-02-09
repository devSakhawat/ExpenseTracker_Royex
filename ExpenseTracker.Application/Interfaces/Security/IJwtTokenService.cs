using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Security;

public interface IJwtTokenService
{
  string GenerateAccessToken(User user);
  string GenerateRefreshToken();
  string HashToken(string token);
}
