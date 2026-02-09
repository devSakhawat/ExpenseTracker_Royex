CREATE OR ALTER PROCEDURE spGetCategoryWiseTotal
AS
BEGIN
  SET NOCOUNT ON;

  SELECT
    c.CategoryId AS [CategoryId],
    c.Name AS [CategoryName],
    ISNULL(SUM(e.Amount), 0) AS [TotalAmount],
    COUNT(e.ExpenseId) AS [ExpenseCount]
  FROM Categories c
  LEFT JOIN Expenses e ON c.CategoryId = e.CategoryId
  WHERE c.IsActive = 1
  GROUP BY c.CategoryId, c.Name
  ORDER BY [TotalAmount] DESC;
END
