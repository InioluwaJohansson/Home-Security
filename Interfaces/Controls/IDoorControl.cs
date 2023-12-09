using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;

namespace Home_Security.Interfaces.Controls;
public interface IDoorControl
{
    public Task<BaseResponse> CreateDoor(GetAuthControlInfoDto getAuthControlInfoDto, CreateDoorDto createDoorDto);
    public Task<BaseResponse> UpdateDoor(GetAuthControlInfoDto getAuthControlInfoDto, UpdateDoorDto updateDoorDto);
    public Task<BaseResponse> UnlockDoor(GetAuthControlInfoDto getAuthControlInfoDto, UpdateDoorDto updateDoorDto);
    public Task<BaseResponse> LockDoor(GetAuthControlInfoDto getAuthControlInfoDto, UpdateDoorDto updateDoorDto);
    public Task<DoorResponseModel> GetDoorById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<DoorsResponseModel> GetAllDoorsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int doorId);
    public Task<DoorsResponseModel> GetAllDoorsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId);
    public Task<DoorsResponseModel> GetAllDoorsByDoorType(GetAuthControlInfoDto getAuthControlInfoDto, DoorType doorType);
    public Task<DoorsResponseModel> GetAllDoors(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteDoor(GetAuthControlInfoDto getAuthControlInfoDto, int doorId);
}
