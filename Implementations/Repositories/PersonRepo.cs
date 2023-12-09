using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Home_Security.Implementations.Repositories;
public class PersonRepo : BaseRepository<Person>, IPersonRepo
{
    public PersonRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Person> GetById(int id)
    {
        return await context.Person.Include(x => x.PersonDetails).Include(x => x.PersonDetails.ContactDetails).Include(x => x.PersonDetails.Addresses).Include(x => x.User).Include(x => x.User.UserRole).Include(x => x.PersonDetails.FingerPrints).FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<List<Person>> List()
    {
        return await context.Person.Include(x => x.PersonDetails).Include(x => x.PersonDetails.ContactDetails).Include(x => x.PersonDetails.Addresses).Include(x => x.User).Include(x => x.User.UserRole).Include(x => x.PersonDetails.FingerPrints).ToListAsync();
    }
}