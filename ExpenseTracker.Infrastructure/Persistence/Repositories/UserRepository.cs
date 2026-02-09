using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
  public UserRepository(ExpenseTrackerContext context) : base(context) { }

  public async Task<User?> GetByLoginIdAsync(string loginId, bool trackChanges = false)
  {
    return trackChanges
      ? await _context.Users.FirstOrDefaultAsync(u => u.LoginId == loginId)
      : await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.LoginId == loginId);
  }
}
