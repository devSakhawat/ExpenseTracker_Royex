using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Infrastructure.Persistence.Context;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
  private readonly ExpenseTrackerContext _context;
  private readonly Lazy<ICategoryRepository> _categoryRepository;
  private readonly Lazy<IExpenseRepository> _expenseRepository;
  private readonly Lazy<IUserRepository> _userRepository;
  private readonly Lazy<IRefreshTokenRepository> _refreshTokenRepository;

  public RepositoryManager(ExpenseTrackerContext context)
  {
    _context = context;
    _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(context));
    _expenseRepository = new Lazy<IExpenseRepository>(() => new ExpenseRepository(context));
    _userRepository = new Lazy<IUserRepository>(() => new UserRepository(context));
    _refreshTokenRepository = new Lazy<IRefreshTokenRepository>(() => new RefreshTokenRepository(context));
  }

  public ICategoryRepository Categories => _categoryRepository.Value;
  public IExpenseRepository Expenses => _expenseRepository.Value;
  public IUserRepository Users => _userRepository.Value;
  public IRefreshTokenRepository RefreshTokens => _refreshTokenRepository.Value;

  public async Task SaveAsync() => await _context.SaveChangesAsync();
}
