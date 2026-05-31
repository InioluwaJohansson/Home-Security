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
    ILogService _logService;
    IPersonRepo _personRepo;
    IActionRepo _actionRepo;
    ILogRepo _logRepo;
    ICameraRepo _cameraRepo;
    public DashboardService(IContactRepo contactRepo, IRoomService roomService, ILogService logService, IPersonRepo personRepo, IActionRepo actionRepo, ICameraRepo cameraRepo, ILogRepo logRepo)
    {
        _contactRepo = contactRepo;
        _roomService = roomService;
        _logService = logService;
        _personRepo = personRepo;
        _actionRepo = actionRepo;
        _cameraRepo = cameraRepo;
        _logRepo = logRepo;
    }
    public async Task<DashBoardResponseModel> GetDashboardData(int personId)
    {
        var contact = await _contactRepo.GetByExpression(x => x.IsDeleted != false);
        var action = (await _actionRepo.GetByExpression(x => !x.IsDeleted)).Take(2).ToList();
        var logs = (await _logRepo.GetByExpression(x => !x.IsDeleted)).OrderByDescending(x => x.TimeOfAction).Take(3).ToList();
        var person = await _personRepo.Get(x => !x.IsDeleted && x.Id == personId);
        if(person != null){
            var rooms = await _roomService.GetRoomByPersonId(personId);
            List<CameraNameUrl> CameraNamesUrls = new List<CameraNameUrl>();
            foreach(var cameraId in person.CameraIds)
            {
                var camera = await _cameraRepo.Get(x => !x.IsDeleted && x.Id == cameraId);
                if(camera != null)
                {
                    CameraNamesUrls.Add(new CameraNameUrl()
                    {
                        Name = camera.CameraName,
                        StreamUrl = camera.LiveStreamUrl
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
            return new DashBoardResponseModel()
            {
                TotalContacts = contact.Count(),
                Actions = action.Select(x => new GetActionDto()
                {
                    Id = x.Id,
                    ActionName = x.ActionName,
                    ActionDescription = x.Description,
                }).ToList() ?? new List<GetActionDto>(),
                Logs = sortedLogs,
                CameraNamesUrls = CameraNamesUrls,
                Rooms = rooms.Data.ToList(),
                Status = true,
            };
        }
        return new DashBoardResponseModel()
        {
            Status = false,
        };
    }
}