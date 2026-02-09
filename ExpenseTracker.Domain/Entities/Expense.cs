namespace ExpenseTracker.Domain.Entities;

public class Expense : BaseEntity
{
  public long ExpenseId { get; set; }
  public int CategoryId { get; set; }
  public decimal Amount { get; set; }
  public DateTime ExpenseDate { get; set; }
  public string? Description { get; set; }

  // Navigation
  public Category Category { get; set; } = null!;
}
