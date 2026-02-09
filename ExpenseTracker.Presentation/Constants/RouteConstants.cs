namespace ExpenseTracker.Presentation.Constants;

public static class RouteConstants
{
  public const string BaseRoute = "api";

  // Auth
  public const string Register = "register";
  public const string Login = "login";
  public const string RefreshToken = "refresh-token";
  public const string RevokeToken = "revoke-token";
  public const string UserInfo = "user-info";

  // Category
  public const string GetCategories = "categories";
  public const string GetCategoryById = "categories/{key:int}";
  public const string CreateCategory = "categories";
  public const string UpdateCategory = "categories/{key:int}";
  public const string DeleteCategory = "categories/{key:int}";

  // Expense
  public const string GetExpenses = "expenses/summary";
  public const string GetExpenseById = "expenses/{key:long}";
  public const string CreateExpense = "expenses/create";
  public const string UpdateExpense = "expenses/{key:long}";
  public const string DeleteExpense = "expenses/{key:long}";

  // Reports
  public const string MonthlySummary = "reports/monthly-summary";
  public const string CategoryWiseTotal = "reports/category-wise-total";
}
