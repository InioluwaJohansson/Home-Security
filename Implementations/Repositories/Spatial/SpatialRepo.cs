
using Home_Security.Context;
using Home_Security.Interfaces.Repositories.Spatial;

namespace Home_Security.Implementations.Repositories.Spatial;

public class SpatialRepository<TEntity> : BaseRepository<TEntity>, ISpatialRepository<TEntity> where TEntity : class, new()
{
    public SpatialRepository(HomeSecurityContext _context)
    {
        _context = context;
    }
}
