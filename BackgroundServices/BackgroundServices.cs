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
        var context = scope.ServiceProvider.GetRequiredService<HomeSecurityContext>();
        await context.Database.MigrateAsync();
        var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();
        await personService.InjectDefaultUser();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthControl>();
        await authService.RunAllFacilities();
        var hardwareService = scope.ServiceProvider.GetRequiredService<IHardwareService>();
        await hardwareService.CheckHardwareStatus();
        Task.Delay(180000, stoppingToken).Wait();
    }
}