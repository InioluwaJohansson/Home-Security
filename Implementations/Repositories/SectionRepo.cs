using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Implementations.Repositories;
using Home_Security.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
public class SectionRepo : BaseRepository<Section>, ISectionRepo
{
    public SectionRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Section> GetById(int id)
    {
        return await context.Section.Include(x => x.Room).Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }
    public async Task<List<Section>> GetBySectionName(string SectionName)
    {
        return await context.Section.Include(x => x.Room).Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).Where(x => x.SectionName.StartsWith(SectionName) && x.IsDeleted == false).ToListAsync();
    }
    public async Task<List<Section>> List()
    {
        return await context.Section.Include(x => x.Room).Include(x => x.Appliance).Include(x => x.Light).Include(x => x.Door).Include(x => x.Window).Where(x => x.IsDeleted == false).ToListAsync();
    }
}