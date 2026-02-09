using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence.Context;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
  public CategoryRepository(ExpenseTrackerContext context) : base(context) { }
}
