using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;
using Microsoft.AspNetCore.SignalR;

namespace Home_Security.Implementations.Services;
public class LogService : ILogService
{
    ILogRepo _logRepo;
    IPersonRepo _personRepo;
    IRealtimeNotificationService _realtime;
    public LogService(ILogRepo logRepo, IPersonRepo personRepo, IRealtimeNotificationService realtime)
    {
        _logRepo = logRepo;
        _personRepo = personRepo;
        _realtime = realtime;
    }
    public async Task<BaseResponse> CreateLog(CreateLogDto createLogDto)
    {
        if (createLogDto != null)
        {
            var log = new Logs()
            {
                PersonId = createLogDto.PersonId,
                TimeOfAction = DateTime.Now,
                LogDetails = createLogDto.LogDetails,
                ActionType = createLogDto.ActionType,
                FacilityType = createLogDto.FacilityType,
                FacilityId = createLogDto.FacilityId,
                CreatedOn = DateTime.Now,
                CreatedBy = createLogDto.PersonId,
                LastModifiedBy = createLogDto.PersonId,
                LastModifiedOn = DateTime.Now,
                IsDeleted = false
            };
            await _logRepo.Create(log);
            var logData = await _logRepo.Get(x => x.CreatedOn == log.CreatedOn);
            await _realtime.NotifyAll("LogCreated", await GetDetails(logData));
            return new BaseResponse()
            {
                Status = true,
                Message = "Action Logged Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Log Action!"
        };
    }
    public async Task<LogsResponseModel> GetLogsByPersonId(int personId, int page = 1, int pageSize = 50)
    {
        var logs = await _logRepo.GetByExpression(x => x.PersonId == personId && !x.IsDeleted);
        if (logs != null)
        {
            var pagedLogs = logs.OrderByDescending(x => x.TimeOfAction).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var sortedLogs = new List<GetLogDto>();
            foreach (var item in pagedLogs)
            {
                var logDetails = await GetDetails(item);
                sortedLogs.Add(logDetails);
            }
            return new LogsResponseModel()
            {
                Data = sortedLogs,
                Status = true,
                Message = "Logs Retrieved Successfully!"
            };
        }
        return new LogsResponseModel()
        {
            Status = false,
            Message = "Unable To Retrieve Logs!"
        };
    }
    public async Task<LogsResponseModel> GetLogsByDate(DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 50)
    {
        var start = startDate.ToDateTime(TimeOnly.MinValue);
        var end = endDate.ToDateTime(TimeOnly.MaxValue);
        var logs = await _logRepo.GetByExpression(x => x.TimeOfAction >= start && x.TimeOfAction <= end && !x.IsDeleted);
        if (logs != null)
        {
            var pagedLogs = logs.OrderByDescending(x => x.TimeOfAction).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var sortedLogs = new List<GetLogDto>();
            foreach (var item in pagedLogs)
            {
                var logDetails = await GetDetails(item);
                sortedLogs.Add(logDetails);
            }
            return new LogsResponseModel()
            {
                Data = sortedLogs,
                Status = true,
                Message = "Logs Retrieved Successfully!"
            };
        }
        return new LogsResponseModel()
        {
            Status = false,
            Message = "Unable To Retrieve Logs!"
        };
    }
    public async Task<LogsResponseModel> GetAllLogs(int page = 1, int pageSize = 50)
    {
        var logs = await _logRepo.GetByExpression(x => x.IsDeleted == false);

        if (logs != null)
        {
            var pagedLogs = logs.OrderByDescending(x => x.TimeOfAction).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var sortedLogs = new List<GetLogDto>();
            foreach (var item in pagedLogs)
            {
                var logDetails = await GetDetails(item);
                sortedLogs.Add(logDetails);
            }
            return new LogsResponseModel()
            {
                Data = sortedLogs,
                Status = true,
                Message = "Logs Retrieved Successfully!"
            };
        }
        return new LogsResponseModel()
        {
            Status = false,
            Message = "Unable To Retrieve Logs!"
        };
    }
    public async Task<GetLogDto> GetDetails(Logs log)
    {
        var person = await _personRepo.GetById(log.PersonId);
        GetPersonDto getPerson = new GetPersonDto();
        if (person != null)
        {
            getPerson = new GetPersonDto()
            {
                Id = person.Id,
                PersonId = person.PersonId,
                Disabled = person.Disabled,
                GetUserDto = new GetUserDto()
                {
                    Id = person.User.Id,
                    UserName = person.User.UserName,
                    Role = person.User.UserRole.Role,
                    RoleName = person.User.UserRole.Role.ToString()
                },
                GetPersonDetailsDto = new GetPersonDetailsDto()
                {
                    Id = person.PersonDetails.Id,
                    FirstName = person.PersonDetails.FirstName,
                    LastName = person.PersonDetails.LastName,
                    ImageUrl = person.PersonDetails.ImageUrl,
                    Gender = person.PersonDetails.Gender,
                }
            };
        }
        return new GetLogDto()
        {
            Id = log.Id,
            PersonId = log.PersonId,
            TimeOfAction = log.TimeOfAction,
            LogDetails = log.LogDetails,
            ActionType = log.ActionType,
            FacilityId = log.FacilityId ?? 0,
            FacilityType = log.FacilityType.ToString(),
            GetPersonDto = getPerson,
        };
    }
}