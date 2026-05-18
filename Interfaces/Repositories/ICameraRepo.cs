using System.Linq.Expressions;
using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Interfaces.Repositories;
public interface ICameraRepo : IRepo<Camera>
{
    public Task<Camera> GetCameraById(int id);
    public Task<IList<Camera>> GetAllCameras(Expression<Func<Camera, bool>> expression);
}