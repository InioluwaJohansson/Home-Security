using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Home_Security.Implementations.Repositories;
public class ContactRepo : BaseRepository<Contact>, IContactRepo
{
    public ContactRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Contact> GetById(int id)
    {
        return await context.Contact.Include(x => x.ContactDetails).Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == id);   
    }
    public async Task<List<Contact>> GetByFirstName(string name)
    {
        return await context.Contact.Include(x => x.ContactDetails).Include(x => x.Address).Where(x => x.FirstName.StartsWith(name)).ToListAsync();
    }
    public async Task<List<Contact>> GetByLastName(string name)
    {
        return await context.Contact.Include(x => x.ContactDetails).Include(x => x.Address).Where(x => x.LastName.StartsWith(name)).ToListAsync();
    }
    public async Task<List<Contact>> GetByContactCategory(int contactCategory)
    {
        return await context.Contact.Include(x => x.ContactDetails).Include(x => x.Address).Where(x => x.ContactCategory == contactCategory).ToListAsync();
    }
    public async Task<List<Contact>> List()
    {
        return await context.Contact.Include(x => x.ContactDetails).Include(x => x.Address).ToListAsync();
    }
}