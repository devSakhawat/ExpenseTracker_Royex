CREATE OR ALTER PROCEDURE spGetMonthlySummary
  @Year INT = NULL
AS
BEGIN
  SET NOCOUNT ON;

  SELECT
    YEAR(e.ExpenseDate) AS [Year],
    MONTH(e.ExpenseDate) AS [Month],
    DATENAME(MONTH, e.ExpenseDate) AS [MonthName],
    SUM(e.Amount) AS [TotalAmount],
    COUNT(*) AS [ExpenseCount]
  FROM Expenses e
  WHERE (@Year IS NULL OR YEAR(e.ExpenseDate) = @Year)
  GROUP BY YEAR(e.ExpenseDate), MONTH(e.ExpenseDate), DATENAME(MONTH, e.ExpenseDate)
  ORDER BY [Year] DESC, [Month] DESC;
END
