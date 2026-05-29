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
    IRoomService _roomService;
    IPersonRepo _personRepo;
    IActionRepo _actionRepo;
    ILogRepo _logRepo;
    ICameraRepo _cameraRepo;
    public DashboardService(IContactRepo contactRepo, IRoomService roomService, IPersonRepo personRepo, IActionRepo actionRepo, ICameraRepo cameraRepo, ILogRepo logRepo)
    {
        _contactRepo = contactRepo;
        _roomService = roomService;
        _personRepo = personRepo;
        _actionRepo = actionRepo;
        _cameraRepo = cameraRepo;
        _logRepo = logRepo;
    }
    public async Task<DashboardResponseModel> GetDashboardData(int personId)
    {
        var contact = await _contactRepo.GetByExpression(x => x.IsDeleted != false);
        var action = (await _actionRepo.GetByExpression(x => !x.IsDeleted)).Take(2).ToList();
        var logs = (await _logRepo.GetByExpression(x => !x.IsDeleted)).OrderByDescending(x => x.TimeOfAction).Take(3).ToList();
        var person = await _personRepo.GetByExpression(x => !x.IsDeleted && x.Id == personId);
        if(person != null){
            var rooms = await _roomService.GetRoomByPersonId(personId);
            List<CameraNameUrl> CameraNamesUrls = new List<CameraNameUrl>();
            foreach(var cameraId in person.CameraIds)
            {
                var cameras = await _cameraRepo.GetByExpression(x => !x.IsDeleted && x.Id == cameraId);
                if(cameras != null)
                {
                    CameraNamesUrls.Add(new CameraNameUrl()
                    {
                        Name = cameras.Name,
                        StreamUrl = cameras.StreamUrl
                    });
                }
            }
            var sortedLogs = new List<GetLogDto>();
            if(logs != null)
            {
                foreach (var item in logs)
                {
                    var logDetails = await _logService.GetDetails(item);
                    sortedLogs.Add(logDetails);
                }
            }
            return new DashboardResponseModel()
            {
                TotalContacts = contact.Count() ?? 0,
                Actions = action.Select(x => new GetActionsDto()
                {
                    Id = action.Id,
                    ActionName = action.ActionName,
                    ActionDescription = action.Description,
                }).ToList() ?? new List<GetActionsDto>(),
                Logs = sortedLogs,
                CameraNamesUrls = CameraNamesUrls,
                Rooms = rooms.Data,
                Status = true,
            };
        }
        return new DashboardResponseModel()
        {
            Status = false,
        };
    }
}