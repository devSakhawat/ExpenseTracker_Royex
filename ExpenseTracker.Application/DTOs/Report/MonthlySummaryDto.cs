namespace ExpenseTracker.Application.DTOs.Report;

public class MonthlySummaryDto
{
  public int Year { get; set; }
  public int Month { get; set; }
  public string MonthName { get; set; } = string.Empty;
  public decimal TotalAmount { get; set; }
  public int ExpenseCount { get; set; }
}
