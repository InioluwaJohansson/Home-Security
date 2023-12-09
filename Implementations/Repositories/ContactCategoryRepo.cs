using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Implementations.Repositories;
public class ContactCategoryRepo : BaseRepository<ContactCategory>, IContactCategoryRepo
{
    public ContactCategoryRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
}