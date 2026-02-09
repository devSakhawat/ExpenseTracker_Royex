```markdown
# ExpenseTracker - Royex

An expense tracking application built with .NET 8.0 using Clean Architecture principles.

## Prerequisites
- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

## Setup
1. Update connection string in `appsettings.json`
2. Run database migrations: `dotnet ef database update --project ExpenseTracker.Infrastructure --startup-project ExpenseTracker.Api`
3. Run the application: `dotnet run --project ExpenseTracker.Api`

## Project Structure
- **ExpenseTracker.Api** - Web API layer
- **ExpenseTracker.Application** - Business logic
- **ExpenseTracker.Domain** - Domain entities
- **ExpenseTracker.Infrastructure** - Data access
- **ExpenseTracker.Presentation** - Controllers/endpoints
```
