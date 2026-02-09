using ExpenseTracker.Application.DTOs.Category;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface ICategoryService
{
  Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
  Task<CategoryDto> GetCategoryByIdAsync(int categoryId);
  Task<string> CreateCategoryAsync(CategoryCreateDto dto);
  Task<string> UpdateCategoryAsync(int categoryId, CategoryCreateDto dto);
  Task<string> DeleteCategoryAsync(int categoryId);
}
