using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Interfaces.Repositories;
public interface IContactRepo : IRepo<Contact>
{
    public Task<Contact> GetById(int id);
    public Task<List<Contact>> GetByFirstName(string name);
    public Task<List<Contact>> GetByLastName(string name);
    public Task<List<Contact>> GetByContactCategory(int contactCategory);
    public Task<List<Contact>> List();
}