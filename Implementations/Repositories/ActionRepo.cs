using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Action = Home_Security.Entities.Action;
namespace Home_Security.Implementations.Repositories;
public class ActionRepo : BaseRepository<Action>, IActionRepo
{
    public ActionRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Action> GetById(int id)
    {
        return await context.Action.Include(x => x.ActionSteps).FirstOrDefaultAsync(x =>x.Id == id && x.IsDeleted == false);
    }
    public async Task<IList<Action>> GetAllActions()
    {
        return await context.Action.Include(x => x.ActionSteps).Where(x => x.IsDeleted == false).ToListAsync();
    }
}