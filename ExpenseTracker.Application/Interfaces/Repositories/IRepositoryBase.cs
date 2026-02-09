using System.Linq.Expressions;

namespace ExpenseTracker.Application.Interfaces.Repositories;

public interface IRepositoryBase<T> where T : class
{
  void Create(T entity);
  void Update(T entity);
  void Delete(T entity);

  Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false);
  Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);
  Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);
  Task<int> CountAsync(Expression<Func<T, bool>>? expression = null);
  Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
}
