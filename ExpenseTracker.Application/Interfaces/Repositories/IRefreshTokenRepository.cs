using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
{
  Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId);
  Task RemoveExpiredTokensAsync();
}
