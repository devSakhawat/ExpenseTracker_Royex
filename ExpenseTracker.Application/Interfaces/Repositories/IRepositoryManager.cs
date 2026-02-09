namespace ExpenseTracker.Application.Interfaces.Repositories;

public interface IRepositoryManager
{
  ICategoryRepository Categories { get; }
  IExpenseRepository Expenses { get; }
  IUserRepository Users { get; }
  IRefreshTokenRepository RefreshTokens { get; }

  Task SaveAsync();
}
