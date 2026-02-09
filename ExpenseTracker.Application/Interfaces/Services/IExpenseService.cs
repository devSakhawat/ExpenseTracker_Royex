using ExpenseTracker.Application.DTOs.Common;
using ExpenseTracker.Application.DTOs.Expense;
using ExpenseTracker.Application.DTOs.Report;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface IExpenseService
{
  Task<PaginatedResult<ExpenseDto>> GetExpensesAsync(PaginationRequest request);
  Task<ExpenseDto> GetExpenseByIdAsync(long expenseId);
  Task<string> CreateExpenseAsync(ExpenseCreateDto dto, int userId);
  Task<string> UpdateExpenseAsync(long expenseId, ExpenseUpdateDto dto, int userId);
  Task<string> DeleteExpenseAsync(long expenseId);
  Task<IEnumerable<MonthlySummaryDto>> GetMonthlySummaryAsync(int? year = null);
  Task<IEnumerable<CategoryWiseTotalDto>> GetCategoryWiseTotalAsync();
}
