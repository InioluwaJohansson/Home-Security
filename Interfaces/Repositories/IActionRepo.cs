using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Action = Home_Security.Entities.Action;
namespace Home_Security.Interfaces.Repositories;
public interface IActionRepo : IRepo<Action>
{
    Task<Action> GetById(int id);
    Task<IList<Action>> GetAllActions();
}