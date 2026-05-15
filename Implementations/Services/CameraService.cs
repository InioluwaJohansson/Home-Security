using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;
using Home_Security.Streaming;

namespace Home_Security.Implementations.Services;

public class CameraService : ICameraService
{
    ICameraRepo _cameraRepo;
    IRealtimeNotificationService _realtime;
    IMqttService _mqtt;
    ICameraStreamingService _cameraStreamingService;

    public CameraService(ICameraRepo cameraRepo, IRealtimeNotificationService realtime, IMqttService mqtt, ICameraStreamingService cameraStreamingService)
    {
        _cameraRepo = cameraRepo;
        _realtime = realtime;
        _mqtt = mqtt;
        _cameraStreamingService = cameraStreamingService;
    }

    private string BuildRtspUrl(CreateCameraDto dto)
    {
        var streamPath = string.IsNullOrWhiteSpace(dto.StreamPath)
            ? "stream1"
            : dto.StreamPath;

        var port = dto.Port == 0 ? 554 : dto.Port;

        return $"rtsp://{dto.Username}:{dto.Password}@{dto.IpAddress}:{port}/{streamPath}";
    }

    public async Task<BaseResponse> CreateCamera(CreateCameraDto createCameraDto, int personId)
    {
        if (createCameraDto == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Unable To Add Camera!"
            };
        }

        var camera = new Camera()
        {
            CameraId = $"CAMERA{Guid.NewGuid().ToString().Replace("-", "")[..5].ToUpper()}",
            CameraName = createCameraDto.CameraName,

            IpAddress = createCameraDto.IpAddress,

            InputStreamUrl = BuildRtspUrl(createCameraDto),

            RecordingPath = $"storage/recordings/{createCameraDto.CameraId}",

            IsOnline = false,
            IsStreaming = false,

            IsActive = false,
            PowerActive = false,

            SectionId = createCameraDto.SectionId,
            RoomId = createCameraDto.RoomId,

            IsDeleted = false,
            CreatedBy = personId,
            LastModifiedBy = personId,
            LastModifiedOn = DateTime.Now
        };

        await _cameraRepo.Create(camera);

        var liveUrl = await _cameraStreamingService.StartStream(camera);

        camera.LiveStreamUrl = liveUrl;
        camera.IsStreaming = true;
        camera.LastStreamStart = DateTime.UtcNow;

        await _cameraRepo.Update(camera);

        await _realtime.NotifyAll("CameraCreated", GetDetails(camera));

        await _mqtt.PublishDeviceUpdate(camera.CameraId, new GenericDeviceUpdateState
        {
            FacilityType = 2,
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
            Message = "Camera Created and Streaming Started!"
        };
    }

    public async Task<BaseResponse> UpdateCamera(UpdateCameraDto updateCameraDto, int personId)
    {
        if (updateCameraDto == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Unable To Update Camera!"
            };
        }

        var camera = await _cameraRepo.Get(x => x.Id == updateCameraDto.Id);

        camera.CameraName = updateCameraDto.CameraName ?? camera.CameraName;
        camera.LastModifiedBy = personId;
        camera.LastModifiedOn = DateTime.Now;

        await _cameraRepo.Update(camera);

        // if (camera.IsStreaming)
        // {
        //     await _cameraStreamingService.RestartStream(camera);
        // }

        await _realtime.NotifyAll("CameraUpdated", GetDetails(camera));

        await _mqtt.PublishDeviceUpdate(camera.CameraId, new GenericDeviceUpdateState
        {
            FacilityType = 2,
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

    public async Task<CameraResponseModel> GetById(int cameraId)
    {
        var camera = await _cameraRepo.Get(x => x.Id == cameraId && x.IsDeleted == false);

        if (camera == null)
        {
            return new CameraResponseModel
            {
                Status = false,
                Message = "Unable To Retrieve Camera!"
            };
        }

        return new CameraResponseModel
        {
            Status = true,
            Message = "Camera Retrieved Successfully!",
            Data = GetDetails(camera),
        };
    }

    public async Task<CamerasResponseModel> GetAllCameras()
    {
        var cameras = await _cameraRepo.GetByExpression(x => x.IsDeleted == false);

        if (cameras == null)
        {
            return new CamerasResponseModel
            {
                Status = false,
                Message = "Unable To Retrieve Cameras!"
            };
        }

        return new CamerasResponseModel
        {
            Status = true,
            Message = "Cameras Retrieved Successfully!",
            Data = cameras.Select(GetDetails).ToList()
        };
    }

    public async Task<CamerasResponseModel> GetAllCamerasByRoomId(int roomId)
    {
        var cameras = await _cameraRepo.GetByExpression(x => x.RoomId == roomId && x.IsDeleted == false);

        return new CamerasResponseModel
        {
            Status = cameras != null,
            Message = cameras != null ? "Cameras Retrieved Successfully!" : "Unable To Retrieve Cameras!",
            Data = cameras?.Select(GetDetails).ToList()
        };
    }

    public async Task<CamerasResponseModel> GetAllCamerasBySectionId(int sectionId)
    {
        var cameras = await _cameraRepo.GetByExpression(x => x.SectionId == sectionId && x.IsDeleted == false);

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

        if (camera == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Unable To Delete Camera!"
            };
        }

        camera.IsActive = false;
        camera.PowerActive = false;
        camera.IsStreaming = false;

        camera.LastModifiedBy = personId;
        camera.LastModifiedOn = DateTime.Now;
        camera.DeletedOn = DateTime.Now;
        camera.DeletedBy = personId;

        await _cameraStreamingService.StopStream(camera);

        await _cameraRepo.Update(camera);

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

    public GetCameraDto GetDetails(Camera camera)
    {
        return new GetCameraDto
        {
            Id = camera.Id,
            CameraName = camera.CameraName,
            CameraId = camera.CameraId,

            IsActive = camera.IsActive,
            PowerActive = camera.PowerActive,

            RoomId = camera.RoomId,
            SectionId = camera.SectionId,

            LastModifiedBy = camera.LastModifiedBy,
            DeletedBy = camera.DeletedBy,
            CreatedBy = camera.CreatedBy,

            LastModifiedOn = camera.LastModifiedOn,
            CreatedOn = camera.CreatedOn,

            LiveStreamUrl = camera.LiveStreamUrl,

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