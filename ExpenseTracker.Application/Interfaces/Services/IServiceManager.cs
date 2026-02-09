namespace ExpenseTracker.Application.Interfaces.Services;

public interface IServiceManager
{
  ICategoryService Categories { get; }
  IExpenseService Expenses { get; }
  IAuthenticationService Authentication { get; }
}
