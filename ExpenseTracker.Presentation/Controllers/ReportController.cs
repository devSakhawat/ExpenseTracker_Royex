using ExpenseTracker.Application.DTOs.Report;
using ExpenseTracker.Application.Helpers;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Presentation.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Presentation.Controllers;

[ApiController]
[Route(RouteConstants.BaseRoute)]
[Authorize]
public class ReportController : ControllerBase
{
  private readonly IServiceManager _serviceManager;

  public ReportController(IServiceManager serviceManager)
  {
    _serviceManager = serviceManager;
  }

  [HttpGet(RouteConstants.MonthlySummary)]
  public async Task<IActionResult> GetMonthlySummary([FromQuery] int? year = null)
  {
    var result = await _serviceManager.Expenses.GetMonthlySummaryAsync(year);
    if (!result.Any())
      return Ok(ResponseHelper.NoContent<IEnumerable<MonthlySummaryDto>>("No data found."));
    return Ok(ResponseHelper.Success(result, "Monthly summary retrieved successfully."));
  }

  [HttpGet(RouteConstants.CategoryWiseTotal)]
  public async Task<IActionResult> GetCategoryWiseTotal()
  {
    var result = await _serviceManager.Expenses.GetCategoryWiseTotalAsync();
    if (!result.Any())
      return Ok(ResponseHelper.NoContent<IEnumerable<CategoryWiseTotalDto>>("No data found."));
    return Ok(ResponseHelper.Success(result, "Category-wise total retrieved successfully."));
  }
}
