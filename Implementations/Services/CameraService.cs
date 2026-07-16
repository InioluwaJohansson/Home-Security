using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;
using Home_Security.Streaming;

namespace Home_Security.Implementations.Services;
public class CameraService : ICameraService
{
    ICameraRepo _cameraRepo;
    IRealtimeNotificationService _realtime;
    IHardwareService _hardwareService;
    IMqttService _mqtt;
    ICameraStreamingService _cameraStreamingService;
    public CameraService(ICameraRepo cameraRepo, IRealtimeNotificationService realtime, IHardwareService hardwareService, IMqttService mqtt, ICameraStreamingService cameraStreamingService)
    {
        _cameraRepo = cameraRepo;
        _realtime = realtime;
        _hardwareService = hardwareService;
        _mqtt = mqtt;
        _cameraStreamingService = cameraStreamingService;
    }
    private string BuildRtspUrl(CreateCameraDto dto)
    {
        Console.WriteLine($"Building RTSP URL for camera: {dto.CameraName}, IP: {dto.IpAddress}, Port: {dto.Port}, StreamPath: {dto.StreamPath}");
        if(dto.Username == null || dto.Password == null)
            return $"rtsp://{dto.IpAddress}:{dto.Port}/{dto.StreamPath}";
        else return $"rtsp://{dto.Username}:{dto.Password}@{dto.IpAddress}:{dto.Port}/{dto.StreamPath}";
    }
    public async Task<BaseResponse> CreateCamera(CreateCameraDto createCameraDto, int personId)
    {
        if (createCameraDto != null)
        {
            string cameraId = $"CAMERA{Guid.NewGuid().ToString().Replace("-", "")[..5].ToUpper()}";
            var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var folderPath = Path.Combine(basePath, "storage");
            var recordingsFolderPath = Path.Combine("Cameras", "Recordings", $"CAM_{cameraId}");
            Directory.CreateDirectory(Path.Combine(folderPath, recordingsFolderPath));
            var camera = new Camera()
            {
                CameraId = cameraId,
                CameraName = createCameraDto.CameraName,
                IpAddress = createCameraDto.IpAddress,
                InputStreamUrl = BuildRtspUrl(createCameraDto),
                LiveStreamUrl = BuildRtspUrl(createCameraDto),
                StreamPath = createCameraDto.StreamPath,
                RecordingPath = recordingsFolderPath,
                IsOnline = false,
                IsStreaming = false,
                IsActive = false,
                PowerActive = false,
                SectionId = createCameraDto.SectionId ?? null,
                RoomId = createCameraDto.RoomId ?? null,
                IsDeleted = false,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now
            };
            await _cameraRepo.Create(camera);
            var liveCamera = await _cameraStreamingService.StartStream(camera);
            camera.LiveStreamUrl = liveCamera.LiveStreamUrl;
            camera.IsStreaming = true;
            camera.LastStreamStart = DateTime.UtcNow;
            var cameraData = GetDetails(await _cameraRepo.Update(camera));
            if(cameraData.IsHidden){ await _realtime.NotifyAll("CameraCreated", new List<Role> { Role.Owner, Role.Wife }, cameraData);}
                else await _realtime.NotifyAll("CameraCreated", cameraData);
            return new BaseResponse
            {
                Status = true,
                Message = "Camera Created and Streaming Started!"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable To Add Camera!"
        };
    }
    public async Task<BaseResponse> UpdateCamera(UpdateCameraDto updateCameraDto, int personId)
    {
        if (updateCameraDto != null)
        {
            var camera = await _cameraRepo.Get(x => x.Id == updateCameraDto.Id);
            if(camera != null){
                camera.CameraName = updateCameraDto.CameraName ?? camera.CameraName;
                camera.IsActive = updateCameraDto.IsActive;
                camera.LastModifiedBy = personId;
                camera.LastModifiedOn = DateTime.Now;
                camera.SectionId = updateCameraDto.SectionId ?? null;
                camera.RoomId = updateCameraDto.RoomId ?? null;
                if (!camera.IsStreaming) camera = await _cameraStreamingService.RestartStream(camera);
                var cameraData = await _cameraRepo.Update(camera);
                if(cameraData.IsHidden){ await _realtime.NotifyAll("CameraUpdated", new List<Role> { Role.Owner, Role.Wife }, GetDetails(cameraData));}
                else await _realtime.NotifyAll("CameraUpdated", GetDetails(cameraData));
                var hardware = await _hardwareService.GetHardwareName(cameraData.Id, FacilityType.Camera);
                await _mqtt.PublishDeviceUpdate(camera.CameraId, new GenericDeviceUpdateState
                {
                    FacilityType = FacilityType.Camera,
                    Id = camera.Id,
                    IsActive = camera.IsActive,
                    PowerActive = camera.PowerActive,
                    IsLocked = false,
                    IsOpen = false,
                    BrightnessLevel = 0
                });
                return new BaseResponse
                {
                    Status = true,
                    Message = "Camera Updated Successfully!"
                };                
            }
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable To Update Camera!"
        };
    }
    public async Task<BaseResponse> RestartCamera(int Id, int personId)
    {
        var camera = await _cameraRepo.Get(x => x.Id == Id);
        if(camera != null){
            if (camera.IsStreaming) camera = await _cameraStreamingService.RestartStream(camera);
            await _cameraRepo.Update(camera);
            if(camera.IsHidden){ await _realtime.NotifyAll("CameraUpdated", new List<Role> { Role.Owner, Role.Wife }, GetDetails(camera));}
            else await _realtime.NotifyAll("CameraUpdated", GetDetails(camera));
            return new BaseResponse
            {
                Status = true,
                Message = "Camera Restarted Successfully!"
            };                
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable To Restart Camera!"
        };
    }
    public async Task<CameraResponseModel> GetById(int cameraId)
    {
        var camera = await _cameraRepo.GetCameraById(cameraId);
        if (camera != null)
        {
            return new CameraResponseModel
            {
                Status = true,
                Message = "Camera Retrieved Successfully!",
                Data = GetDetails(camera),
            };
        }
        return new CameraResponseModel
        {
            Status = false,
            Message = "Unable To Retrieve Camera!"
        };
    }
    public async Task<CamerasResponseModel> GetAllCameras()
    {
        var cameras = await _cameraRepo.GetByExpression(x => x.IsDeleted == false);
        if (cameras != null)
        {
            return new CamerasResponseModel
            {
                Status = true,
                Message = "Cameras Retrieved Successfully!",
                Data = cameras.Select(GetDetails).ToList()
            };
        }
        return new CamerasResponseModel
        {
            Status = false,
            Message = "Unable To Retrieve Cameras!"
        };
    }
    public async Task<CamerasResponseModel> GetAllCamerasByRoomId(int roomId)
    {
        var cameras = await _cameraRepo.GetAllCameras(x => x.RoomId == roomId && x.IsDeleted == false);
        return new CamerasResponseModel
        {
            Status = cameras != null,
            Message = cameras != null ? "Cameras Retrieved Successfully!" : "Unable To Retrieve Cameras!",
            Data = cameras?.Select(GetDetails).ToList()
        };
    }
    public async Task<CamerasResponseModel> GetAllCamerasBySectionId(int sectionId)
    {
        var cameras = await _cameraRepo.GetAllCameras(x => x.SectionId == sectionId && x.IsDeleted == false);
        return new CamerasResponseModel
        {
            Status = cameras != null,
            Message = cameras != null ? "Cameras Retrieved Successfully!" : "Unable To Retrieve Cameras!",
            Data = cameras?.Select(GetDetails).ToList()
        };
    }
    public async Task<BaseResponse> Delete(int cameraId, int personId)
    {
        var camera = await _cameraRepo.Get(x => x.Id == cameraId);
        if (camera != null)
        {
            camera.IsActive = false;
            camera.PowerActive = false;
            camera.IsStreaming = false;
            camera.LastModifiedBy = personId;
            camera.LastModifiedOn = DateTime.Now;
            camera.DeletedOn = DateTime.Now;
            camera.DeletedBy = personId;
            camera.IsDeleted = true;
            await _cameraStreamingService.StopStream(camera);
            await _cameraRepo.Update(camera);
            await _hardwareService.RemoveFacilityItem(camera.Id, FacilityType.Camera);
            await _realtime.NotifyAll("CameraDeleted", new
            {
                camera.Id,
                camera.CameraId
            });
            return new BaseResponse
            {
                Status = true,
                Message = "Camera Deleted Successfully!"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable To Delete Camera!"
        };
    }
    public GetCameraDto GetDetails(Camera camera)
    {
        return new GetCameraDto
        {
            Id = camera.Id,
            CameraName = camera.CameraName,
            CameraId = camera.CameraId,
            IsActive = camera.IsActive,
            PowerActive = camera.PowerActive,
            Port = camera.Port,
            IpAddress = camera.IpAddress,
            Username = camera.Username,
            StreamPath = camera.StreamPath,
            RoomId = camera.RoomId ?? null,
            SectionId = camera.SectionId ?? null,
            LastModifiedBy = camera.LastModifiedBy,
            DeletedBy = camera.DeletedBy,
            CreatedBy = camera.CreatedBy,
            LastModifiedOn = camera.LastModifiedOn,
            CreatedOn = camera.CreatedOn,
            LiveStreamUrl = camera.LiveStreamUrl,
            IsHidden = camera.IsHidden,
            Recordings = camera.Recordings == null
                ? new List<GetRecordingDto>()
                : camera.Recordings.Select(x => new GetRecordingDto
                {
                    FilePath = x.FilePath,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime
                }).ToList()
        };
    }
}