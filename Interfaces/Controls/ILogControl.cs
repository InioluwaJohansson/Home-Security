using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface ILogControl
{
    public Task<LogResponseModel> GetLogById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<LogsResponseModel> GetAllLogsByPersonId(GetAuthControlInfoDto getAuthControlInfoDto, int personId);
    public Task<LogsResponseModel> GetAllLogsByActionType(GetAuthControlInfoDto getAuthControlInfoDto, string actionType);
    public Task<LogsResponseModel> GetAllLogs(GetAuthControlInfoDto getAuthControlInfoDto);
}
