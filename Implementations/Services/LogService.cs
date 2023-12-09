using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class LogService : ILogService
{
    ILogRepo _logRepo;
    IPersonRepo _personRepo;
    public LogService(ILogRepo logRepo, IPersonRepo personRepo)
    {
        _logRepo = logRepo;
        _personRepo = personRepo;
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
                CreatedOn = DateTime.Now,
                CreatedBy = createLogDto.PersonId,
                LastModifiedBy = createLogDto.PersonId,
                LastModifiedOn = DateTime.Now,
                IsDeleted = false
            };
            await _logRepo.Create(log);
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
    public async Task<LogResponseModel> GetLogById(int id)
    {
        var log = await _logRepo.Get(x => x.Id == id && x.IsDeleted == false);
        if (log != null)
        {   
            return new LogResponseModel()
            {
                Data = await GetDetails(log),
                Status = true,
                Message = "Log Retrieved Successfully!"
            };
        }
        return new LogResponseModel()
        {
            Status = false,
            Message = "Unable To Retrieve Log!"
        };
    }
    public async Task<LogsResponseModel> GetLogsByActionType(string actionType)
    {
        var logs = await _logRepo.GetByExpression(x => x.ActionType == actionType && x.IsDeleted == false);
        if (logs != null)
        {
            return new LogsResponseModel()
            {
                Data = ((ICollection<GetLogDto>)logs.Select(async x => await GetDetails(x))).OrderBy(x => x.TimeOfAction).ToList(),
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
    public async Task<LogsResponseModel> GetLogsByPersonId(int personId)
    {
        var logs = await _logRepo.GetByExpression(x => x.PersonId == personId && x.IsDeleted == false);
        if (logs != null)
        {
            return new LogsResponseModel()
            {
                Data = ((ICollection<GetLogDto>)logs.Select(async x => await GetDetails(x))).OrderBy(x => x.TimeOfAction).ToList(),
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
    public async Task<LogsResponseModel> GetAllLogs()
    {
        var logs = await _logRepo.GetByExpression(x => x.IsDeleted == false);
        if (logs != null)
        {
            return new LogsResponseModel()
            {
                Data = ((ICollection<GetLogDto>)logs.Select(async x => await GetDetails(x))).OrderBy(x => x.TimeOfAction).ToList(),
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
        GetPersonDto getPerson = null;
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
            GetPersonDto = getPerson,
        };
    }
}