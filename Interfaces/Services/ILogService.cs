using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface ILogService
{
    public Task<BaseResponse> CreateLog(CreateLogDto createLogDto);
    public Task<LogResponseModel> GetLogById(int id);
    public Task<LogsResponseModel> GetLogsByActionType(string actionType);
    public Task<LogsResponseModel> GetLogsByPersonId(int personId);
    public Task<LogsResponseModel> GetAllLogs();
}