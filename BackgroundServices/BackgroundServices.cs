using System.Diagnostics;
using Home_Security.Context;
using Home_Security.Encryption;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;
using Home_Security.StartUp;
using Home_Security.Streaming;
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
        // Any Other Thing
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
        var startUp = scope.ServiceProvider.GetRequiredService<IStartUpService>();
        await startUp.InitateStartUp();
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
public class MqttBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public MqttBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var mqttService = scope.ServiceProvider.GetRequiredService<IMqttService>();
        await mqttService.ConnectAsync(stoppingToken);
    }
}
public class CameraStreamingBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public CameraStreamingBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var cameraStreamingService = scope.ServiceProvider.GetRequiredService<ICameraStreamingService>();
        await cameraStreamingService.StartProcessingCameras(stoppingToken);
    }
}
public class RecordingCleanUpBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public RecordingCleanUpBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var recordingCleanUpService = scope.ServiceProvider.GetRequiredService<IRecordingCleanUpService>();
        await recordingCleanUpService.SyncAllRecordings(stoppingToken);
    }
}
public class RecordingCleanUpBackgroundServiceDelete : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public RecordingCleanUpBackgroundServiceDelete(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var recordingCleanUpService = scope.ServiceProvider.GetRequiredService<IRecordingCleanUpService>();
        await recordingCleanUpService.ExecuteCleanUp(stoppingToken);
    }
}