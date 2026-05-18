using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Services;
public interface IActionService
{
    public Task<BaseResponse> CreateAction(CreateActionDto createActionDto, int personId);
    public Task<BaseResponse> UpdateAction(UpdateActionDto updateActionDto, int personId);
    public Task<ActionResponseModel> GetById(int actionId);
    public Task<ActionsResponseModel> GetAllActions();
    public Task<BaseResponse> ActivateDeactivateAction(int actionId);
    public Task<BaseResponse> DeleteAction(int actionId, int personId);
    public Task<BaseResponse> AddActionStep(List<CreateActionStepDto> createActionStepDtos, int personId);
    public Task<BaseResponse> UpdateActionStep(List<UpdateActionStepDto> updateActionStepDtos, int personId); 
    public Task<BaseResponse> DeleteActionStep(int actionId, int actionStepId, int personId);
    public Task TriggerActionsForExternal(int externalId);
}