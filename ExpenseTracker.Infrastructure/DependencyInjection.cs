using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Security;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Application.Services;
using ExpenseTracker.Infrastructure.Persistence.Context;
using ExpenseTracker.Infrastructure.Persistence.Repositories;
using ExpenseTracker.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<ExpenseTrackerContext>(options =>
      options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<IRepositoryManager, RepositoryManager>();
    services.AddScoped<IServiceManager, ServiceManager>();
    services.AddSingleton<IPasswordHasher, PasswordHasher>();
    services.AddSingleton<IJwtTokenService, JwtTokenService>();

    return services;
  }
}
