using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface ILogControl
{
    public Task<LogsResponseModel> GetAllLogsByPersonId(GetAuthControlInfoDto getAuthControlInfoDto, int personId, int page = 1, int pageSize = 50);
    public Task<LogsResponseModel> GetAllLogsByDate(GetAuthControlInfoDto getAuthControlInfoDto, DateOnly startDate, DateOnly endDate, int page = 1, int pageSize = 50);
    public Task<LogsResponseModel> GetAllLogs(GetAuthControlInfoDto getAuthControlInfoDto, int page = 1, int pageSize = 50);
}
