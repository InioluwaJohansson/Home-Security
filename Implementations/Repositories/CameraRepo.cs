using System.Linq.Expressions;
using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Home_Security.Implementations.Repositories;
public class CameraRepo : BaseRepository<Camera>, ICameraRepo
{
    public CameraRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Camera> GetCameraById(int id)
    {
        return await context.Camera.Include(x => x.Recordings).Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    public async Task<IList<Camera>> GetAllCameras(Expression<Func<Camera, bool>> expression)
    {
        return await context.Camera.Include(x => x.Recordings).Where(expression).ToListAsync();
    }
}