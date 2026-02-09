using ExpenseTracker.Application.Interfaces.Services;

namespace ExpenseTracker.Api.BackgroundServices;

public class TokenCleanupBackgroundService : BackgroundService
{
  private readonly IServiceProvider _serviceProvider;
  private readonly ILogger<TokenCleanupBackgroundService> _logger;

  public TokenCleanupBackgroundService(IServiceProvider serviceProvider, ILogger<TokenCleanupBackgroundService> logger)
  {
    _serviceProvider = serviceProvider;
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        using var scope = _serviceProvider.CreateScope();
        var serviceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();
        await serviceManager.Authentication.RemoveExpiredTokensAsync();
        _logger.LogInformation("Expired tokens cleaned up at {Time}", DateTime.UtcNow);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error cleaning up expired tokens.");
      }

      await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
    }
  }
}
