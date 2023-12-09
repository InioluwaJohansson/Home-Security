using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface ICameraService
{
    public Task<BaseResponse> CreateCamera(CreateCameraDto createCameraDto);
    public Task<BaseResponse> UpdateCamera(UpdateCameraDto updateCameraDto);
    public Task<CameraResponseModel> GetById(int cameraId);
    public Task<CamerasResponseModel> GetAllCameras();
    public Task<CamerasResponseModel> GetAllCamerasByRoomId(int roomId);
    public Task<CamerasResponseModel> GetAllCamerasBySectionId(int sectionId);
    public Task<BaseResponse> Delete(int cameraId, int personId);
}