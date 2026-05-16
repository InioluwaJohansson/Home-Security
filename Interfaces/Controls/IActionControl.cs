using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface IActionControl
{
    public Task<BaseResponse> CreateAction(GetAuthControlInfoDto getAuthControlInfoDto, CreateActionDto createActionDto);
    public Task<BaseResponse> UpdateAction(GetAuthControlInfoDto getAuthControlInfoDto, UpdateActionDto updateActionDto);
    public Task<ActionResponseModel> GetActionById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<ActionsResponseModel> GetAllActionsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId);
    public Task<ActionsResponseModel> GetAllActionsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId);
    public Task<ActionsResponseModel> GetAllActions(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteAction(GetAuthControlInfoDto getAuthControlInfoDto, int actionId);
}
