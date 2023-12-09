using Home_Security.Context;

namespace Home_Security;
public class BackgroundServices : BackgroundService
{
    IServiceScopeFactory _serviceScopeFactory;
    public BackgroundServices(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var contaxt = scope.ServiceProvider.GetRequiredService<HomeSecurityContext>();
        await Task.CompletedTask;
    }
}
