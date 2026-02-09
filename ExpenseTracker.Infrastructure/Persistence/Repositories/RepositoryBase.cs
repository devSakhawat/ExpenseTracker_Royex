using System.Linq.Expressions;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
  protected readonly ExpenseTrackerContext _context;

  protected RepositoryBase(ExpenseTrackerContext context)
  {
    _context = context;
  }

  public void Create(T entity) => _context.Set<T>().Add(entity);
  public void Update(T entity) => _context.Set<T>().Update(entity);
  public void Delete(T entity) => _context.Set<T>().Remove(entity);

  public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false)
  {
    return trackChanges
      ? await _context.Set<T>().FirstOrDefaultAsync(predicate)
      : await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
  }

  public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? orderBy = null, bool trackChanges = false)
  {
    var query = trackChanges ? _context.Set<T>().AsQueryable() : _context.Set<T>().AsNoTracking();
    if (orderBy != null) query = query.OrderBy(orderBy);
    return await query.ToListAsync();
  }

  public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false)
  {
    var query = trackChanges ? _context.Set<T>().Where(expression) : _context.Set<T>().AsNoTracking().Where(expression);
    if (orderBy != null) query = query.OrderBy(orderBy);
    return await query.ToListAsync();
  }

  public async Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
  {
    return expression == null
      ? await _context.Set<T>().CountAsync()
      : await _context.Set<T>().CountAsync(expression);
  }

  public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
  {
    return await _context.Set<T>().AnyAsync(expression);
  }
}
