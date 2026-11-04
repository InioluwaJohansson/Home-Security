namespace Home_Security.BackgroundServices;
public interface IRecordingCleanUpService
{
    Task ExecuteCleanUp(CancellationToken stoppingToken);
    Task SyncAllRecordings(CancellationToken stoppingToken);
}