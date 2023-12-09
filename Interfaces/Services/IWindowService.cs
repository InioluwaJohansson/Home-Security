using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IWindowService
{
    public Task<BaseResponse> CreateWindow(CreateWindowDto createWindowDto);
    public Task<BaseResponse> UpdateWindow(UpdateWindowDto updateWindowDto);
    public Task<WindowResponseModel> GetById(int windowId);
    public Task<WindowsResponseModel> GetAllWindows();
    public Task<WindowsResponseModel> GetAllWindowsByRoomId(int roomId);
    public Task<WindowsResponseModel> GetAllWindowsBySectionId(int sectionId);
    public Task<BaseResponse> Delete(int windowId, int personId);
}