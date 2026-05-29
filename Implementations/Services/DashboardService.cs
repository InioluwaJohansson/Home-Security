using Home_Security.Entities;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Authentication;
namespace Home_Security.Implementations.Services;
public class DashboardService : IDashboardService
{
    IContactRepo _contactRepo;
    IRoomRepo _roomRepo;
    IPersonRepo _personRepo;
    IActionRepo _actionRepo;
    ILogRepo _logRepo;
    ICameraRepo _cameraRepo;
    public DashboardService(IContactRepo contactRepo, IRoomRepo roomRepo, IPersonRepo personRepo, IActionRepo actionRepo, ICameraRepo cameraRepo, ILogRepo logRepo)
    {
        _contactRepo = contactRepo;
        _roomRepo = roomRepo;
        _personRepo = personRepo;
        _actionRepo = actionRepo;
        _cameraRepo = cameraRepo;
        _logRepo = logRepo;
    }
    public async Task<DashboardResponseModel> GetDashboardDataAsync(int personId)
    {
        var contact = await _contactRepo.GetByExpression(x => x.IsDeleted != false);
        var action = await _actionRepo.GetByExpression(x => !x.IsDeleted);
        var log = await _logRepo.GetByExpression(x => !x.IsDeleted);
        var camera = await _cameraRepo.GetByExpression(x => !x.IsDeleted);
        return new DashboardResponseModel()
        {
            TotalContacts = contact.Count(),
            GetActionsDtos = 
        }
    }
}