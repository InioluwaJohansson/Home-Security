using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Implementations.Repositories;
public class CameraRepo : BaseRepository<Camera>, ICameraRepo
{
    public CameraRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Camera> GetByCameraIdAsync(string id)
    {
        return await context.Cameras.Include(x => x.Recording)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
}