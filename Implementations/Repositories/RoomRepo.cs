using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Home_Security.Implementations.Repositories;
public class RoomRepo : BaseRepository<Room>, IRoomRepo
{
    public RoomRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Room> GetById(int id)
    {
        return await context.Room.Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }
    public async Task<List<Room>> GetByRoomName(string roomName)
    {
        return await context.Room.Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).Where(x => x.RoomName.StartsWith(roomName) && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<Room>> GetBySectionId(int sectionId)
    {
        return await context.Room.Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).Where(x => x.SectionId == sectionId && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<Room>> List()
    {
        return await context.Room.Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).Where(x => x.IsDeleted == false).ToListAsync();
    }
}