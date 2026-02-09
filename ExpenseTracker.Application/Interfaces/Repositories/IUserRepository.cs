using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
  Task<User?> GetByLoginIdAsync(string loginId, bool trackChanges = false);
}
