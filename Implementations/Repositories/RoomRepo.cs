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
        return await context.Room.Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).Include(x => x.Camera).ThenInclude(c => c.Recordings).Include(x => x.External).SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }
    public async Task<List<Room>> GetByRoomName(string roomName)
    {
        return await context.Room.Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).Include(x => x.Camera).ThenInclude(c => c.Recordings).Include(x => x.External).Where(x => x.RoomName.StartsWith(roomName) && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<Room>> GetBySectionId(int sectionId)
    {
        return await context.Room.Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).Include(x => x.Camera).ThenInclude(c => c.Recordings).Include(x => x.External).Where(x => x.SectionId == sectionId && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<Room>> GetByPersonId(int personId)
    {
        return await context.Room.Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).Include(x => x.Camera).ThenInclude(c => c.Recordings).Include(x => x.External).Where(x => x.PersonId == personId && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<Room>> List()
    {
        return await context.Room.Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).Include(x => x.Camera).ThenInclude(c => c.Recordings).Include(x => x.External).Where(x => x.IsDeleted == false).ToListAsync();
    }
}