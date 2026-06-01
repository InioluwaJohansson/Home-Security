using Home_Security.Context;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Home_Security.BackgroundServices;
public class BackgroundServices : BackgroundService
{
    IServiceScopeFactory _serviceScopeFactory;
    public BackgroundServices(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateAsyncScope();
        
        
        // var hardwareService = scope.ServiceProvider.GetRequiredService<IHardwareService>();
        // await hardwareService.CheckHardwareStatus();
        await Task.CompletedTask;
    }
}
public class StartupInitializer : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public StartupInitializer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HomeSecurityContext>();
        await context.Database.MigrateAsync();
        var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();
        await personService.InjectDefaultUser();
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
public class AuthServices : BackgroundService
{
    IServiceScopeFactory _serviceScopeFactory;
    public AuthServices(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateAsyncScope();
        var authControl = scope.ServiceProvider.GetRequiredService<IAuthControl>();
        await authControl.RunAllFacilities();
        await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
    }
}
public class HardwareServices : BackgroundService
{
    IServiceScopeFactory _serviceScopeFactory;
    public HardwareServices(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateAsyncScope();
        var hardwareService = scope.ServiceProvider.GetRequiredService<IHardwareService>();
        await hardwareService.CheckHardwareStatus();
        await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
    }
}