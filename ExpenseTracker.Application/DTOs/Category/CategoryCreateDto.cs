using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Application.DTOs.Category;

public class CategoryCreateDto
{
  [Required(ErrorMessage = "Category name is required.")]
  [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
  public string Name { get; set; } = string.Empty;

  [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
  public string? Description { get; set; }
}
