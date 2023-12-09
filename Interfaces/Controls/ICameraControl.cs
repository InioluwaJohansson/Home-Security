using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;
public interface ICameraControl
{
    public Task<BaseResponse> CreateCamera(GetAuthControlInfoDto getAuthControlInfoDto, CreateCameraDto createCameraDto);
    public Task<BaseResponse> UpdateCamera(GetAuthControlInfoDto getAuthControlInfoDto, UpdateCameraDto updateCameraDto);
    public Task<CameraResponseModel> GetCameraById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<CamerasResponseModel> GetAllCamerasBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId);
    public Task<CamerasResponseModel> GetAllCamerasByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId);
    public Task<CamerasResponseModel> GetAllCameras(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteCamera(GetAuthControlInfoDto getAuthControlInfoDto, int cameraId);
}
