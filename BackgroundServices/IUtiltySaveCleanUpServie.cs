using Home_Security.Models.DTOs;

namespace Home_Security.BackgroundServices;

public interface IUtilitySaveCleanUpService
{
    Task ExecuteCleanUp(CancellationToken stoppingToken);
    Task ExecuteUtilityCheck(CancellationToken stoppingToken);
    Task SaveUtilityCacheData(UtilityDataMQTT utilityDataMQTT);
}