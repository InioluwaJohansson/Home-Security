using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;

namespace Home_Security.Interfaces.Services;
public interface IDoorService
{
    public Task<BaseResponse> CreateDoor(CreateDoorDto createDoorDto);
    public Task<BaseResponse> UpdateDoor(UpdateDoorDto updateDoorDto);
    public Task<DoorResponseModel> GetById(int doorId);
    public Task<DoorsResponseModel> GetAllDoors();
    public Task<DoorsResponseModel> GetAllDoorsByDoorType(DoorType doorType);
    public Task<DoorsResponseModel> GetAllDoorsByRoomId(int roomId);
    public Task<DoorsResponseModel> GetAllDoorsBySectionId(int sectionId);
    public Task<BaseResponse> Delete(int doorId, int personId);
}