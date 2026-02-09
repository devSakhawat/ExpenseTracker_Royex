using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Application.DTOs.Expense;

public class ExpenseUpdateDto
{
  [Required(ErrorMessage = "CategoryId is required.")]
  public int CategoryId { get; set; }

  [Required(ErrorMessage = "Amount is required.")]
  [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
  public decimal Amount { get; set; }

  [Required(ErrorMessage = "Expense date is required.")]
  public DateTime ExpenseDate { get; set; }

  [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
  public string? Description { get; set; }
}
