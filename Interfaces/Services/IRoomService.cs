using Home_Security.Entities;
using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IRoomService
{
    public Task<BaseResponse> CreateRoom(CreateRoomDto createRoomDto, int personId);
    public Task<BaseResponse> UpdateRoom(UpdateRoomDto updateRoomDto, int personId);
    public Task<RoomResponseModel> GetRoomById(int id);
    public Task<RoomsResponseModel> GetRoomByPersonId(int id);
    public Task<RoomsResponseModel> GetRoomByRoomName(string roomName);
    public Task<RoomsResponseModel> GetAllRoomsBySectionId(int sectionId);
    public Task<RoomsResponseModel> GetAllRooms();
    public Task<BaseResponse> Delete(int id, int personId);
    public GetRoomDto GetDetails(Room room);
}
