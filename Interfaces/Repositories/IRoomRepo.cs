using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Interfaces.Repositories;
public interface IRoomRepo : IRepo<Room>
{
    public Task<Room> GetById(int id);
    public Task<List<Room>> GetByRoomName(string roomName);
    public Task<List<Room>> GetBySectionId(int sectionId);
    public Task<List<Room>> List();
}