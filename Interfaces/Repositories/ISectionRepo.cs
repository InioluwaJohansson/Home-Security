using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Interfaces.Repositories;
public interface ISectionRepo : IRepo<Section>
{
    public Task<Section> GetById(int id);
    public Task<List<Section>> GetBySectionName(string SectionName);
    public Task<List<Section>> List();
}