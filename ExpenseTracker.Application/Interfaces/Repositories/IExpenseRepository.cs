using ExpenseTracker.Application.DTOs.Report;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Repositories;

public interface IExpenseRepository : IRepositoryBase<Expense>
{
  Task<IEnumerable<Expense>> GetExpensesWithCategoryAsync(int pageNumber, int pageSize, string? searchTerm = null, string? sortColumn = null, string? sortDirection = null);
  Task<IEnumerable<MonthlySummaryDto>> GetMonthlySummaryAsync(int? year = null);
  Task<IEnumerable<CategoryWiseTotalDto>> GetCategoryWiseTotalAsync();
}
