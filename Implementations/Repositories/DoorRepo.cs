using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Implementations.Repositories;
public class DoorRepo : BaseRepository<Door>, IDoorRepo
{
    public DoorRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
}