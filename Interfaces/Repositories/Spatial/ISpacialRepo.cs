
using Home_Security.Interfaces.Repositories;

namespace Home_Security.Interfaces.Repositories.Spatial;

public interface ISpatialRepository<TEntity> : IRepo<TEntity>
    where TEntity : class, new()
{
}
