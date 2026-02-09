using ExpenseTracker.Application.Constants;
using ExpenseTracker.Application.DTOs.Category;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Exceptions;

namespace ExpenseTracker.Application.Services;

public class CategoryService : ICategoryService
{
  private readonly IRepositoryManager _repository;

  public CategoryService(IRepositoryManager repository)
  {
    _repository = repository;
  }

  public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
  {
    var categories = await _repository.Categories.GetAllAsync(orderBy: c => c.Name);

    return categories.Select(c => new CategoryDto
    {
      CategoryId = c.CategoryId,
      Name = c.Name,
      Description = c.Description,
      IsActive = c.IsActive,
      CreatedDate = c.CreatedDate
    });
  }

  public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId)
  {
    var category = await _repository.Categories.GetByIdAsync(c => c.CategoryId == categoryId)
      ?? throw new NotFoundException(nameof(Category), categoryId);

    return new CategoryDto
    {
      CategoryId = category.CategoryId,
      Name = category.Name,
      Description = category.Description,
      IsActive = category.IsActive,
      CreatedDate = category.CreatedDate
    };
  }

  public async Task<string> CreateCategoryAsync(CategoryCreateDto dto)
  {
    var exists = await _repository.Categories.ExistsAsync(c => c.Name == dto.Name);
    if (exists)
      throw new ConflictException($"Category with name '{dto.Name}' already exists.");

    var category = new Category
    {
      Name = dto.Name,
      Description = dto.Description,
      IsActive = true,
      CreatedDate = DateTime.UtcNow
    };

    _repository.Categories.Create(category);
    await _repository.SaveAsync();

    return OperationMessage.Created;
  }

  public async Task<string> UpdateCategoryAsync(int categoryId, CategoryCreateDto dto)
  {
    var category = await _repository.Categories.GetByIdAsync(c => c.CategoryId == categoryId, trackChanges: true)
      ?? throw new NotFoundException(nameof(Category), categoryId);

    var duplicateExists = await _repository.Categories.ExistsAsync(c => c.Name == dto.Name && c.CategoryId != categoryId);
    if (duplicateExists)
      throw new ConflictException($"Category with name '{dto.Name}' already exists.");

    category.Name = dto.Name;
    category.Description = dto.Description;
    category.UpdatedDate = DateTime.UtcNow;

    _repository.Categories.Update(category);
    await _repository.SaveAsync();

    return OperationMessage.Updated;
  }

  public async Task<string> DeleteCategoryAsync(int categoryId)
  {
    var category = await _repository.Categories.GetByIdAsync(c => c.CategoryId == categoryId, trackChanges: true)
      ?? throw new NotFoundException(nameof(Category), categoryId);

    var hasExpenses = await _repository.Expenses.ExistsAsync(e => e.CategoryId == categoryId);
    if (hasExpenses)
      throw new ConflictException("Cannot delete category with associated expenses.");

    _repository.Categories.Delete(category);
    await _repository.SaveAsync();

    return OperationMessage.Deleted;
  }
}
