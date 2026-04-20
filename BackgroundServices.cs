using Home_Security.Context;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Microsoft.EntityFrameworkCore;

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
        using var scope = _serviceScopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<HomeSecurityContext>();
        await context.Database.MigrateAsync();
        var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();
        await personService.InjectDefaultUser();
    }
}