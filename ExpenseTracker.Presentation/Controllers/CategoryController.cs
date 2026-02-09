using ExpenseTracker.Application.DTOs.Category;
using ExpenseTracker.Application.Helpers;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Presentation.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Presentation.Controllers;

[ApiController]
[Route(RouteConstants.BaseRoute)]
[Authorize]
public class CategoryController : ControllerBase
{
  private readonly IServiceManager _serviceManager;

  public CategoryController(IServiceManager serviceManager)
  {
    _serviceManager = serviceManager;
  }

  [HttpGet(RouteConstants.GetCategories)]
  public async Task<IActionResult> GetCategories()
  {
    var categories = await _serviceManager.Categories.GetAllCategoriesAsync();
    if (!categories.Any())
      return Ok(ResponseHelper.NoContent<IEnumerable<CategoryDto>>("No categories found."));
    return Ok(ResponseHelper.Success(categories, "Categories retrieved successfully."));
  }

  [HttpGet(RouteConstants.GetCategoryById)]
  public async Task<IActionResult> GetCategoryById([FromRoute] int key)
  {
    var category = await _serviceManager.Categories.GetCategoryByIdAsync(key);
    return Ok(ResponseHelper.Success(category, "Category retrieved successfully."));
  }

  [HttpPost(RouteConstants.CreateCategory)]
  public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto dto)
  {
    var result = await _serviceManager.Categories.CreateCategoryAsync(dto);
    return Ok(ResponseHelper.Created(result, "Category created successfully."));
  }

  [HttpPut(RouteConstants.UpdateCategory)]
  public async Task<IActionResult> UpdateCategory([FromRoute] int key, [FromBody] CategoryCreateDto dto)
  {
    var result = await _serviceManager.Categories.UpdateCategoryAsync(key, dto);
    return Ok(ResponseHelper.Success(result, "Category updated successfully."));
  }

  [HttpDelete(RouteConstants.DeleteCategory)]
  public async Task<IActionResult> DeleteCategory([FromRoute] int key)
  {
    var result = await _serviceManager.Categories.DeleteCategoryAsync(key);
    return Ok(ResponseHelper.Success(result, "Category deleted successfully."));
  }
}
