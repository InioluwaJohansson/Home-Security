using Home_Security.Context;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Interfaces.Repositories;
public interface IUserRepo : IRepo<User>
{
    public Task<User> GetUserById(int id);
    public Task<User> GetUserByUserName(string userName);
}