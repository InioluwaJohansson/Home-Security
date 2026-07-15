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
                CleanUpOldHls();
            }
            catch{}
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
    private void CleanupOldRecordings()
    {
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var recordingsFolderPath = Path.Combine(basePath, "storage", "Cameras", "Recordings");
        if (!Directory.Exists(recordingsFolderPath))
            return;
        var cameraFolders = Directory.GetDirectories(recordingsFolderPath);
        foreach (var folder in cameraFolders)
        {
            var files = Directory.GetFiles(folder, "*.mp4");
            foreach (var file in files)
            {
                try
                {
                    var creationTime = File.GetCreationTimeUtc(file);
                    if (creationTime < DateTime.UtcNow.AddDays(-7)) File.Delete(file);
                }
                catch{}
            }
        }
    }
    private void CleanUpOldHls()
    {
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var hlsFolderPath = Path.Combine(basePath, "storage", "Cameras", "hls");
        if (!Directory.Exists(hlsFolderPath))
            return;
        var cameraFolders = Directory.GetDirectories(hlsFolderPath);
        foreach (var folder in cameraFolders)
        {
            var files = Directory.GetFiles(folder, "*.ts");
            foreach (var file in files)
            {
                try
                {
                    var creationTime = File.GetCreationTimeUtc(file);
                    if (creationTime < DateTime.UtcNow.AddDays(-7)) File.Delete(file);
                }
                catch{}
            }
        }
    }
}