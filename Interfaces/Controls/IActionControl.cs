using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface IActionControl
{
    public Task<BaseResponse> CreateAction(GetAuthControlInfoDto getAuthControlInfoDto, CreateActionDto createActionDto);
    public Task<BaseResponse> UpdateAction(GetAuthControlInfoDto getAuthControlInfoDto, UpdateActionDto updateActionDto);
    public Task<ActionResponseModel> GetActionById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<BaseResponse> ActivateDeactivateAction(GetAuthControlInfoDto getAuthControlInfoDto, int actionId);
    public Task<ActionsResponseModel> GetAllActions(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteAction(GetAuthControlInfoDto getAuthControlInfoDto, int actionId);
    public Task<BaseResponse> AddActionStep(GetAuthControlInfoDto getAuthControlInfoDto, List<CreateActionStepDto> createActionStepDtos);
    public Task<BaseResponse> UpdateActionStep(GetAuthControlInfoDto getAuthControlInfoDto, List<UpdateActionStepDto> updateActionStepDtos); 
    public Task<BaseResponse> DeleteActionStep(GetAuthControlInfoDto getAuthControlInfoDto, int actionId, int actionStepId);
}
