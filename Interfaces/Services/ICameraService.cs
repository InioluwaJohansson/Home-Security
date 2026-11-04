using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface ICameraService
{
    public Task<BaseResponse> CreateCamera(CreateCameraDto createCameraDtom, int personId);
    public Task<BaseResponse> UpdateCamera(UpdateCameraDto updateCameraDto, int personId);
    public Task<BaseResponse> ToggleCamera(UpdateCameraDto updateCameraDto, int personId);
    public Task<BaseResponse> RestartCamera(int Id, int personId);
    public Task<CameraResponseModel> GetById(int cameraId);
    public Task<CamerasResponseModel> GetAllCameras();
    public Task<CamerasResponseModel> GetAllCamerasByRoomId(int roomId);
    public Task<CamerasResponseModel> GetAllCamerasBySectionId(int sectionId);
    public Task<BaseResponse> Delete(int cameraId, int personId);
}