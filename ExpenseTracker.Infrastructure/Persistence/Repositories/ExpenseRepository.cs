using ExpenseTracker.Application.DTOs.Report;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories;

public class ExpenseRepository : RepositoryBase<Expense>, IExpenseRepository
{
  public ExpenseRepository(ExpenseTrackerContext context) : base(context) { }

  public async Task<IEnumerable<Expense>> GetExpensesWithCategoryAsync(int pageNumber, int pageSize, string? searchTerm = null, string? sortColumn = null, string? sortDirection = null)
  {
    var query = _context.Expenses.AsNoTracking().Include(e => e.Category).AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
      var term = searchTerm.ToLower();
      query = query.Where(e => (e.Description != null && e.Description.ToLower().Contains(term)) || e.Category.Name.ToLower().Contains(term));
    }

    query = sortColumn?.ToLower() switch
    {
      "amount" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(e => e.Amount) : query.OrderBy(e => e.Amount),
      "date" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(e => e.ExpenseDate) : query.OrderBy(e => e.ExpenseDate),
      "category" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(e => e.Category.Name) : query.OrderBy(e => e.Category.Name),
      _ => query.OrderByDescending(e => e.ExpenseDate)
    };

    return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
  }

  public async Task<IEnumerable<MonthlySummaryDto>> GetMonthlySummaryAsync(int? year = null)
  {
    var param = new SqlParameter("@Year", (object?)year ?? DBNull.Value);
    return await _context.Database
      .SqlQueryRaw<MonthlySummaryDto>("EXEC spGetMonthlySummary @Year", param)
      .ToListAsync();
  }

  public async Task<IEnumerable<CategoryWiseTotalDto>> GetCategoryWiseTotalAsync()
  {
    return await _context.Database
      .SqlQueryRaw<CategoryWiseTotalDto>("EXEC spGetCategoryWiseTotal")
      .ToListAsync();
  }
}
