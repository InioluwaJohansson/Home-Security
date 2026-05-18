using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Implementations.Repositories;
public class RecordingRepo : BaseRepository<Recording>, IRecordingRepo
{
    public RecordingRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
}