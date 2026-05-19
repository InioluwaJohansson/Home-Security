using Microsoft.Extensions.Hosting;
namespace Home_Security.BackgroundServices;
public class RecordingCleanupService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                CleanupOldRecordings();
            }
            catch
            {
                // TODO: add logging here
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private void CleanupOldRecordings()
    {
        string recordingsRoot = "storage/recordings";
        if (!Directory.Exists(recordingsRoot))
            return;
        var cameraFolders = Directory.GetDirectories(recordingsRoot);
        foreach (var folder in cameraFolders)
        {
            var files = Directory.GetFiles(folder, "*.mp4");
            foreach (var file in files)
            {
                try
                {
                    var creationTime = File.GetCreationTimeUtc(file);
                    if (creationTime < DateTime.UtcNow.AddDays(-7))
                    {
                        File.Delete(file);
                    }
                }
                catch
                {
                    // ignore individual file errors
                }
            }
        }
    }
}