using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Interfaces.Repositories;
public interface IPersonRepo : IRepo<Person>
{
    public Task<Person> GetById(int id);
    public Task<List<Person>> List();
}