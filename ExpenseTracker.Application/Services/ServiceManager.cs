using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Security;
using ExpenseTracker.Application.Interfaces.Services;

namespace ExpenseTracker.Application.Services;

public sealed class ServiceManager : IServiceManager
{
  private readonly Lazy<ICategoryService> _categoryService;
  private readonly Lazy<IExpenseService> _expenseService;
  private readonly Lazy<IAuthenticationService> _authenticationService;

  public ServiceManager(IRepositoryManager repository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
  {
    _categoryService = new Lazy<ICategoryService>(() => new CategoryService(repository));
    _expenseService = new Lazy<IExpenseService>(() => new ExpenseService(repository));
    _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(repository, passwordHasher, jwtTokenService));
  }

  public ICategoryService Categories => _categoryService.Value;
  public IExpenseService Expenses => _expenseService.Value;
  public IAuthenticationService Authentication => _authenticationService.Value;
}
