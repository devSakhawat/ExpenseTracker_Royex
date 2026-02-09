namespace ExpenseTracker.Application.DTOs.Expense;

public class ExpenseDto
{
  public long ExpenseId { get; set; }
  public int CategoryId { get; set; }
  public string CategoryName { get; set; } = string.Empty;
  public decimal Amount { get; set; }
  public DateTime ExpenseDate { get; set; }
  public string? Description { get; set; }
  public DateTime CreatedDate { get; set; }
}
