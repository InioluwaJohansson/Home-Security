using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Home_Security.Implementations.Repositories;
public class FingerPrintRepo : BaseRepository<FingerPrint>, IFingerPrintRepo
{
    public FingerPrintRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<FingerPrint> GetFingerPrintById(int id)
    {
        return await context.FingerPrint.Include(x => x.Person).Include(x => x.Person.PersonDetails).Include(x => x.Person.PersonDetails.ContactDetails).Include(x => x.Person.PersonDetails.Addresses).Include(x => x.Person.User).Include(x => x.Person.User.UserRole).FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<FingerPrint> GetFingerPrintByBinaryData(BinaryData data)
    {
        return await context.FingerPrint.Include(x => x.Person).Include(x => x.Person.PersonDetails).Include(x => x.Person.PersonDetails.ContactDetails).Include(x => x.Person.PersonDetails.Addresses).Include(x => x.Person.User).Include(x => x.Person.User.UserRole).FirstOrDefaultAsync(x => x.FingerPrintEncoding.Equals(data));
    }
}