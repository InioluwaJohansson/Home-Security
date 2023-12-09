using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Home_Security.Implementations.Repositories;
public class UserRepo : BaseRepository<User>, IUserRepo
{
    public UserRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<User> GetUserById(int id)
    {
        return await context.Users.Include(x => x.UserRole).FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }
    public async Task<User> GetUserByUserName(string userName)
    {
        return await context.Users.Include(x => x.UserRole).FirstOrDefaultAsync(x => x.UserName == userName && x.IsDeleted == false);
    }
}
