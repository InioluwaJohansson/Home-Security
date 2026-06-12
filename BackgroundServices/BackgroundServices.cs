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
private readonly IConfiguration _configuration;

    public StartupInitializer(IServiceScopeFactory scopeFactory,IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
_configuration = configuration;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HomeSecurityContext>();
        await context.Database.MigrateAsync();
        var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();
        await personService.InjectDefaultUser();
var person = _configuration
    .GetSection("SeedPerson")
    .Get<CreatePersonDto>();
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
public class AuthServices : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuthServices(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var authControl = scope.ServiceProvider.GetRequiredService<IAuthControl>();
                await authControl.RunAllFacilities();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Background service error: {ex.Message}");
            }
            await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
        }
    }
}
public class HardwareServices : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HardwareServices(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var hardwareService = scope.ServiceProvider.GetRequiredService<IHardwareService>();
                await hardwareService.CheckHardwareStatus();
                var microControllerService = scope.ServiceProvider.GetRequiredService<IMicroControllerService>();
                await microControllerService.SendUpdatedHardwareData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HardwareServices error: {ex.Message}");
            }
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}