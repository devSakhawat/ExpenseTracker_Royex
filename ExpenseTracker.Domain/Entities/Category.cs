namespace ExpenseTracker.Domain.Entities;

public class Category : BaseEntity
{
  public int CategoryId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  public bool IsActive { get; set; } = true;

  // Navigation
  public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
