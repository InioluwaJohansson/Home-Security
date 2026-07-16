using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;
using Microsoft.Extensions.Hosting;
namespace Home_Security.BackgroundServices;
public class RecordingCleanUpService : IRecordingCleanUpService
{
    public IRecordingRepo _recordingRepo;
    public ICameraRepo _cameraRepo;
    public IRealtimeNotificationService _realtimeNotificationService;
    public RecordingCleanUpService(IRecordingRepo recordingRepo, ICameraRepo cameraRepo, IRealtimeNotificationService realtimeNotificationService)
    {
        _recordingRepo = recordingRepo;
        _cameraRepo = cameraRepo;
        _realtimeNotificationService = realtimeNotificationService;
    }
    public async Task ExecuteCleanUp(CancellationToken stoppingToken)
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
    public async Task SyncAllRecordings(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SyncRecordings(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
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
    public async Task SyncRecordings(CancellationToken cancellationToken = default)
    {
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var cameras = await _cameraRepo.GetByExpression(x => !x.IsDeleted);
        foreach (var camera in cameras)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var recordingFolder = Path.Combine(basePath,"storage", camera.RecordingPath);
            if (!Directory.Exists(recordingFolder))
                continue;
            var files = Directory.GetFiles(recordingFolder, "*.mp4").OrderBy(x => x);
            foreach (var file in files)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var existing = await _recordingRepo.Get(x => x.FilePath == file);
                if (existing != null)
                    continue;
                var info = new FileInfo(file);
                DateTime startTime;
                try
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    var timestamp = fileName.Replace($"{camera.CameraId}_", "");
                    startTime = DateTime.ParseExact(timestamp, "yyyy-MM-dd_HH-mm-ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    startTime = info.CreationTimeUtc;
                }
                var recording = new Recording
                {
                    CameraId = camera.Id,
                    FileName = info.Name,
                    FilePath = file,
                    StartTime = startTime,
                    EndTime = startTime.AddMinutes(5), // Because segment_time = 300
                    CreatedOn = DateTime.UtcNow,
                    LastModifiedOn = DateTime.UtcNow,
                    IsDeleted = false
                };
                recording = await _recordingRepo.Create(recording);
                await _realtimeNotificationService.NotifyAll("NewRecording", GetDetails(recording));
            }
        }
    }
    private GetRecordingDto GetDetails(Recording recording)
    {
        return new GetRecordingDto
        {
            CameraId = recording.CameraId,
            FileName = recording.FileName,
            FilePath = recording.FilePath,
            StartTime = recording.StartTime,
            EndTime = recording.EndTime
        };
    }
}