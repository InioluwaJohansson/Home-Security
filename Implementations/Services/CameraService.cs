using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class CameraService : ICameraService
{
    ICameraRepo _cameraRepo;
    public CameraService(ICameraRepo cameraRepo)
    {
        _cameraRepo = cameraRepo;
    }
    public async Task<BaseResponse> CreateCamera(CreateCameraDto createCameraDto)
    {
        if (createCameraDto != null)
        {
            var camera = new Camera()
            {
                CameraId = $"CAMERA{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                CameraName = createCameraDto.CameraName,
                IsActive = createCameraDto.IsActive,
                PowerActive = createCameraDto.PowerActive,
                SectionId = createCameraDto.SectionId,
                RoomId = createCameraDto.RoomId,
                IsDeleted = false,
                CreatedBy = createCameraDto.personId,
                LastModifiedBy = createCameraDto.personId,
                LastModifiedOn = DateTime.Now
            };
            await _cameraRepo.Create(camera);
            return new BaseResponse()
            {
                Status = true,
                Message = "Camera Successfully Added!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Camera!"
        };
    }
    public async Task<BaseResponse> UpdateCamera(UpdateCameraDto updateCameraDto)
    {
        if (updateCameraDto != null)
        {
            var camera = await _cameraRepo.Get(x => x.Id == updateCameraDto.Id);
            camera.CameraName = updateCameraDto.CameraName ?? camera.CameraName;
            camera.IsActive = updateCameraDto.IsActive;
            camera.PowerActive = updateCameraDto.PowerActive;
            camera.LastModifiedBy = updateCameraDto.personId;
            camera.LastModifiedOn = DateTime.Now;
            await _cameraRepo.Update(camera);
            return new BaseResponse()
            {
                Status = true,
                Message = "Camera Updated Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Camera!"
        };
    }
    public async Task<CameraResponseModel> GetById(int cameraId)
    {
        var camera = await _cameraRepo.Get(x => x.Id == cameraId && x.IsDeleted == false);
        if (camera != null)
        {
            return new CameraResponseModel()
            {
                Data = GetDetails(camera),
                Status = true,
                Message = "Camera Retrieved Successfully!"
            };
        }
        return new CameraResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Camera!"
        };
    }
    public async Task<CamerasResponseModel> GetAllCameras()
    {
        var cameras = await _cameraRepo.GetByExpression(x => x.IsDeleted == false);
        if (cameras != null)
        {
            return new CamerasResponseModel()
            {
                Data = cameras.Select(x => GetDetails(x)).ToList(),
                Message = "Cameras Retrieved Successfully!",
                Status = true
            };
        }
        return new CamerasResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Cameras!",
            Status = false
        };
    }
    public async Task<CamerasResponseModel> GetAllCamerasByRoomId(int roomId)
    {
        var cameras = await _cameraRepo.GetByExpression(x => x.RoomId == roomId && x.IsDeleted == false);
        if (cameras != null)
        {
            return new CamerasResponseModel()
            {
                Data = cameras.Select(x => GetDetails(x)).ToList(),
                Message = "Cameras Retrieved Successfully!",
                Status = true
            };
        }
        return new CamerasResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Cameras!",
            Status = false
        };
    }
    public async Task<CamerasResponseModel> GetAllCamerasBySectionId(int sectionId)
    {
        var cameras = await _cameraRepo.GetByExpression(x => x.SectionId == sectionId && x.IsDeleted == false);
        if (cameras != null)
        {
            return new CamerasResponseModel()
            {
                Data = cameras.Select(x => GetDetails(x)).ToList(),
                Message = "Cameras Retrieved Successfully!",
                Status = true
            };
        }
        return new CamerasResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Cameras!",
            Status = false
        };
    }
    public async Task<BaseResponse> Delete(int cameraId, int personId)
    {
        var camera = await _cameraRepo.Get(x => x.Id == cameraId);
        if (camera != null)
        {
            camera.IsActive = false;
            camera.PowerActive = false;
            camera.LastModifiedBy = personId;
            camera.LastModifiedOn = DateTime.Now;
            camera.DeletedOn = DateTime.Now;
            camera.DeletedBy = personId;
            await _cameraRepo.Update(camera);
            return new BaseResponse()
            {
                Status = true,
                Message = "Camera Deleted Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Camera!"
        };
    }
    public GetCameraDto GetDetails(Camera camera)
    {
        return new GetCameraDto()
        {
            Id = camera.Id,
            CameraName = camera.CameraName,
            IsActive = camera.IsActive,
            PowerActive = camera.PowerActive,
            RoomId = camera.RoomId,
            SectionId = camera.SectionId,
            CameraId = camera.CameraId,
            LastModifiedBy = camera.LastModifiedBy,
            DeletedBy = camera.DeletedBy,
            CreatedBy = camera.CreatedBy,
            LastModifiedOn = camera.LastModifiedOn,
            CreatedOn = camera.CreatedOn,
            IsDeleted = camera.IsDeleted,
        };
    }
}