using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface IRoomControl
{
    public Task<BaseResponse> CreateRoom(GetAuthControlInfoDto getAuthControlInfoDto, CreateRoomDto createRoomDto);
    public Task<BaseResponse> UpdateRoom(GetAuthControlInfoDto getAuthControlInfoDto, UpdateRoomDto updateRoomDto);
    public Task<RoomResponseModel> GetRoomById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<RoomsResponseModel> GetRoomByRoomName(GetAuthControlInfoDto getAuthControlInfoDto, string roomName);
    public Task<RoomsResponseModel> GetAllRoomsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId);
    public Task<RoomsResponseModel> GetAllRooms(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteRoom(GetAuthControlInfoDto getAuthControlInfoDto, int roomId);
}
