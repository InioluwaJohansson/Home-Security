using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Implementations.Services;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Controls;
public class CameraControl : ICameraControl
{
    IAuthControl _authControl;
    ICameraService _cameraService;
    ILogService _logService;
    public CameraControl(IAuthControl authControl, ICameraService cameraService, ILogService logService)
    {
        _authControl = authControl;
        _cameraService = cameraService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateCamera(GetAuthControlInfoDto getAuthControlInfoDto, CreateCameraDto createCameraDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var camera = await _cameraService.CreateCamera(createCameraDto, auth.Id);
            if (camera.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A Camera was newly added. Awaiting Physical Interfacing!";
                await _logService.CreateLog(log);
            }
            return camera;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateCamera(GetAuthControlInfoDto getAuthControlInfoDto, UpdateCameraDto updateCameraDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var camera = await _cameraService.UpdateCamera(updateCameraDto, auth.Id);
            if (camera.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateCameraDto.CameraName} Camera was recently updated!";
                await _logService.CreateLog(log);
            }
            return camera;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<CameraResponseModel> GetCameraById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var camera = await _cameraService.GetById(id);
            camera.Data = DtoRefactor(camera.Data);
            if (camera.Data.IsHidden && auth.Role == Role.Owner || auth.Role == Role.Wife)
                return camera;
            else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
            {
                return new CameraResponseModel()
                {
                    Data = null,
                    Status = _authControl.AuthFaliure().Status,
                    Message = "Unauthorized Action"
                };
            }
        }
        return new CameraResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<CamerasResponseModel> GetAllCamerasBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var camera = await _cameraService.GetAllCamerasBySectionId(sectionId);
            camera.Data = camera.Data.Select(x => DtoRefactor(x)).ToList();
            return camera;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var camera = await _cameraService.GetAllCamerasBySectionId(sectionId);
            camera.Data = camera.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return camera;
        }
        return new CamerasResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<CamerasResponseModel> GetAllCamerasByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var camera = await _cameraService.GetAllCamerasByRoomId(roomId);
            camera.Data = camera.Data.Select(x => DtoRefactor(x)).ToList();
            return camera;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var camera = await _cameraService.GetAllCamerasByRoomId(roomId);
            camera.Data = camera.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return camera;
        }
        return new CamerasResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<CamerasResponseModel> GetAllCameras(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var camera = await _cameraService.GetAllCameras();
            camera.Data = camera.Data.Select(x => DtoRefactor(x)).ToList();
            return camera;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var camera = await _cameraService.GetAllCameras();
            camera.Data = camera.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return camera;
        }
        return new CamerasResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteCamera(GetAuthControlInfoDto getAuthControlInfoDto, int cameraId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var camera = await _cameraService.Delete(cameraId, auth.Id);
            if(camera.Status == true)
            {
                var getCamera = _authControl.GetCameraName(cameraId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getCamera} Camera was Deleted!";
                await _logService.CreateLog(log);
            }            
            return camera;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public GetCameraDto DtoRefactor(GetCameraDto x)
    {
        x.LastModifiedByName = _authControl.GetUserPersonName(x.LastModifiedBy);
        x.PeronName = _authControl.GetUserPersonName(x.personId);
        x.CreatedByName = _authControl.GetUserPersonName(x.CreatedBy);
        x.RoomName = _authControl.GetRoomName(x.RoomId ?? 0);
        x.SectionName = _authControl.GetSectionName(x.SectionId ?? 0);
        return x;
    }
}