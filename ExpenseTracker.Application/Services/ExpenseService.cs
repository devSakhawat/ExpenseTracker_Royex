using ExpenseTracker.Application.Constants;
using ExpenseTracker.Application.DTOs.Common;
using ExpenseTracker.Application.DTOs.Expense;
using ExpenseTracker.Application.DTOs.Report;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Exceptions;

namespace ExpenseTracker.Application.Services;

public class ExpenseService : IExpenseService
{
  private readonly IRepositoryManager _repository;

  public ExpenseService(IRepositoryManager repository)
  {
    _repository = repository;
  }

  public async Task<PaginatedResult<ExpenseDto>> GetExpensesAsync(PaginationRequest request)
  {
    var totalCount = await _repository.Expenses.CountAsync();
    var expenses = await _repository.Expenses.GetExpensesWithCategoryAsync(
      request.PageNumber, request.PageSize, request.SearchTerm, request.SortColumn, request.SortDirection);

    var items = expenses.Select(e => new ExpenseDto
    {
      ExpenseId = e.ExpenseId,
      CategoryId = e.CategoryId,
      CategoryName = e.Category?.Name ?? string.Empty,
      Amount = e.Amount,
      ExpenseDate = e.ExpenseDate,
      Description = e.Description,
      CreatedDate = e.CreatedDate
    });

    return new PaginatedResult<ExpenseDto>
    {
      Items = items,
      TotalCount = totalCount,
      PageNumber = request.PageNumber,
      PageSize = request.PageSize
    };
  }

  public async Task<ExpenseDto> GetExpenseByIdAsync(long expenseId)
  {
    var expense = await _repository.Expenses.GetByIdAsync(e => e.ExpenseId == expenseId)
      ?? throw new NotFoundException(nameof(Expense), expenseId);

    var category = await _repository.Categories.GetByIdAsync(c => c.CategoryId == expense.CategoryId);

    return new ExpenseDto
    {
      ExpenseId = expense.ExpenseId,
      CategoryId = expense.CategoryId,
      CategoryName = category?.Name ?? string.Empty,
      Amount = expense.Amount,
      ExpenseDate = expense.ExpenseDate,
      Description = expense.Description,
      CreatedDate = expense.CreatedDate
    };
  }

  public async Task<string> CreateExpenseAsync(ExpenseCreateDto dto, int userId)
  {
    var categoryExists = await _repository.Categories.ExistsAsync(c => c.CategoryId == dto.CategoryId && c.IsActive);
    if (!categoryExists)
      throw new NotFoundException(nameof(Category), dto.CategoryId);

    var expense = new Expense
    {
      CategoryId = dto.CategoryId,
      Amount = dto.Amount,
      ExpenseDate = dto.ExpenseDate,
      Description = dto.Description,
      CreatedDate = DateTime.UtcNow,
      CreatedBy = userId
    };

    _repository.Expenses.Create(expense);
    await _repository.SaveAsync();
    return OperationMessage.Created;
  }

  public async Task<string> UpdateExpenseAsync(long expenseId, ExpenseUpdateDto dto, int userId)
  {
    var expense = await _repository.Expenses.GetByIdAsync(e => e.ExpenseId == expenseId, trackChanges: true)
      ?? throw new NotFoundException(nameof(Expense), expenseId);

    var categoryExists = await _repository.Categories.ExistsAsync(c => c.CategoryId == dto.CategoryId && c.IsActive);
    if (!categoryExists)
      throw new NotFoundException(nameof(Category), dto.CategoryId);

    expense.CategoryId = dto.CategoryId;
    expense.Amount = dto.Amount;
    expense.ExpenseDate = dto.ExpenseDate;
    expense.Description = dto.Description;
    expense.UpdatedDate = DateTime.UtcNow;
    expense.UpdatedBy = userId;

    _repository.Expenses.Update(expense);
    await _repository.SaveAsync();
    return OperationMessage.Updated;
  }

  public async Task<string> DeleteExpenseAsync(long expenseId)
  {
    var expense = await _repository.Expenses.GetByIdAsync(e => e.ExpenseId == expenseId, trackChanges: true)
      ?? throw new NotFoundException(nameof(Expense), expenseId);

    _repository.Expenses.Delete(expense);
    await _repository.SaveAsync();
    return OperationMessage.Deleted;
  }

  public async Task<IEnumerable<MonthlySummaryDto>> GetMonthlySummaryAsync(int? year = null)
  {
    return await _repository.Expenses.GetMonthlySummaryAsync(year);
  }

  public async Task<IEnumerable<CategoryWiseTotalDto>> GetCategoryWiseTotalAsync()
  {
    return await _repository.Expenses.GetCategoryWiseTotalAsync();
  }
}
