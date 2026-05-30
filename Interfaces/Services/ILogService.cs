using Home_Security.Entities;
using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface ILogService
{
    public Task<BaseResponse> CreateLog(CreateLogDto createLogDto);
    public Task<LogsResponseModel> GetLogsByDate(DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 50);
    public Task<LogsResponseModel> GetLogsByPersonId(int personId, int page = 1, int pageSize = 50);
    public Task<LogsResponseModel> GetAllLogs(int page = 1, int pageSize = 50);
    public Task<GetLogDto> GetDetails(Logs log);
}