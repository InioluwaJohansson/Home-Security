using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;

namespace Home_Security.Interfaces.Controls;
public interface IWindowControl
{
    public Task<BaseResponse> CreateWindow(GetAuthControlInfoDto getAuthControlInfoDto, CreateWindowDto createWindowDto);
    public Task<BaseResponse> UpdateWindow(GetAuthControlInfoDto getAuthControlInfoDto, UpdateWindowDto updateWindowDto);
    public Task<BaseResponse> UnlockWindow(GetAuthControlInfoDto getAuthControlInfoDto, UpdateWindowDto updateWindowDto);
    public Task<BaseResponse> LockWindow(GetAuthControlInfoDto getAuthControlInfoDto, UpdateWindowDto updateWindowDto);
    public Task<WindowResponseModel> GetWindowById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<WindowsResponseModel> GetAllWindowsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int windowId);
    public Task<WindowsResponseModel> GetAllWindowsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId);
    public Task<WindowsResponseModel> GetAllWindows(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteWindow(GetAuthControlInfoDto getAuthControlInfoDto, int windowId);
}
