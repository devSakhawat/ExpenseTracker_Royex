using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
{
  public RefreshTokenRepository(ExpenseTrackerContext context) : base(context) { }

  public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId)
  {
    return await _context.RefreshTokens
      .Where(t => t.UserId == userId && !t.IsRevoked && t.ExpiryDate > DateTime.UtcNow)
      .ToListAsync();
  }

  public async Task RemoveExpiredTokensAsync()
  {
    var expiredTokens = await _context.RefreshTokens
      .Where(t => t.ExpiryDate <= DateTime.UtcNow || t.IsRevoked)
      .ToListAsync();

    if (expiredTokens.Any())
      _context.RefreshTokens.RemoveRange(expiredTokens);
  }
}
