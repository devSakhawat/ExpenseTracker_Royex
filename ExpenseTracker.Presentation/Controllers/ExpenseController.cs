using System.Security.Claims;
using ExpenseTracker.Application.DTOs.Common;
using ExpenseTracker.Application.DTOs.Expense;
using ExpenseTracker.Application.Helpers;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Presentation.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Presentation.Controllers;

[ApiController]
[Route(RouteConstants.BaseRoute)]
[Authorize]
public class ExpenseController : ControllerBase
{
  private readonly IServiceManager _serviceManager;

  public ExpenseController(IServiceManager serviceManager)
  {
    _serviceManager = serviceManager;
  }

  [HttpPost(RouteConstants.GetExpenses)]
  public async Task<IActionResult> GetExpenses([FromBody] PaginationRequest request)
  {
    var result = await _serviceManager.Expenses.GetExpensesAsync(request);
    if (!result.Items.Any())
      return Ok(ResponseHelper.NoContent<PaginatedResult<ExpenseDto>>("No expenses found."));
    return Ok(ResponseHelper.Success(result, "Expenses retrieved successfully."));
  }

  [HttpGet(RouteConstants.GetExpenseById)]
  public async Task<IActionResult> GetExpenseById([FromRoute] long key)
  {
    var result = await _serviceManager.Expenses.GetExpenseByIdAsync(key);
    return Ok(ResponseHelper.Success(result, "Expense retrieved successfully."));
  }

  [HttpPost(RouteConstants.CreateExpense)]
  public async Task<IActionResult> CreateExpense([FromBody] ExpenseCreateDto dto)
  {
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    var result = await _serviceManager.Expenses.CreateExpenseAsync(dto, userId);
    return Ok(ResponseHelper.Created(result, "Expense created successfully."));
  }

  [HttpPut(RouteConstants.UpdateExpense)]
  public async Task<IActionResult> UpdateExpense([FromRoute] long key, [FromBody] ExpenseUpdateDto dto)
  {
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    var result = await _serviceManager.Expenses.UpdateExpenseAsync(key, dto, userId);
    return Ok(ResponseHelper.Success(result, "Expense updated successfully."));
  }

  [HttpDelete(RouteConstants.DeleteExpense)]
  public async Task<IActionResult> DeleteExpense([FromRoute] long key)
  {
    var result = await _serviceManager.Expenses.DeleteExpenseAsync(key);
    return Ok(ResponseHelper.Success(result, "Expense deleted successfully."));
  }
}
