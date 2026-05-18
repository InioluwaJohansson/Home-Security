using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Action = Home_Security.Entities.Action;
namespace Home_Security.Implementations.Services;

public class ActionService : IActionService
{
    IActionRepo _actionRepo;
    IExternalRepo _externalRepo;
    IApplianceService _applianceService;
    ILightService _lightService;
    ICameraService _cameraService;
    IDoorService _doorService;
    IWindowService _windowService;
    ILogService _logService;
    public ActionService(IActionRepo actionRepo, IExternalRepo externalRepo, IApplianceService applianceService, ILightService lightService, ICameraService cameraService, IDoorService doorService, IWindowService windowService, ILogService logService)
    {
        _actionRepo = actionRepo;
        _externalRepo = externalRepo;
        _applianceService = applianceService;
        _lightService = lightService;
        _cameraService = cameraService;
        _doorService = doorService;
        _windowService = windowService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateAction(CreateActionDto createActionDto, int personId)
    {
        if(createActionDto != null)
        {
            var action = new Action()
            {
                ActionName = createActionDto.ActionName,
                Description = createActionDto.Description,
                ActionId = $"ACTION{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}"
            };
            await _actionRepo.Create(action);
            return new BaseResponse
            {
                Status = true,
                Message = "Action Added Successfully!"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable to Add Action!"
        };
    }
    public async Task<BaseResponse> UpdateAction(UpdateActionDto updateActionDto, int personId)
    {
        var action = await _actionRepo.GetById(updateActionDto.Id);
        if(action != null)
        {
            action.ActionName = updateActionDto.ActionName ?? action.ActionName;
            action.Description = updateActionDto.Description ?? action.Description;
            action.LastModifiedBy = personId;
            action.LastModifiedOn = DateTime.UtcNow;
            await _actionRepo.Update(action);
            return new BaseResponse
            {
                Status = true,
                Message = "Action Updated Successfully!"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable to Update Action!"
        };
    }
    public async Task<BaseResponse> ActivateDeactivateAction(int actionId)
    {
        var action = await _actionRepo.Get(x => x.Id == actionId && x.IsDeleted == false);
        if (action != null)
        {
            if(action.IsActive == true) action.IsActive = false;
            else if(action.IsActive == false){
                action.IsActive = true;
                await TriggerActions(actionId);
            }
            await _actionRepo.Update(action);
            return new BaseResponse
            {
                Status = true,
                Message = "Action Activated",
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable to Activate Action!"
        };
    }
    public async Task<ActionResponseModel> GetById(int actionId)
    {
        var action = await _actionRepo.GetById(actionId);
        if (action != null)
        {
            return new ActionResponseModel
            {
                Data = GetDetails(action),
                Status = true,
                Message = "Action Retrieved!",
            };
        }
        return new ActionResponseModel
        {
            Status = false,
            Message = "Unable to Retrieve Action!"
        };
    }
    public async Task<ActionsResponseModel> GetAllActions()
    {
        var actions = await _actionRepo.GetAllActions();
        if (actions != null)
        {
            return new ActionsResponseModel
            {
                Data = actions.Select(x => GetDetails(x)).ToList(),
                Status = true,
                Message = "Action Retrieved!",
            };
        }
        return new ActionsResponseModel
        {
            Status = false,
            Message = "Unable to Retrieve Action!"
        };
    }
    public async Task<BaseResponse> DeleteAction(int actionId, int personId)
    {
        var action = await _actionRepo.GetById(actionId);
        if (action != null)
        {
            action.IsDeleted = true;
            action.DeletedBy = personId;
            foreach(var actionStep in action.ActionSteps)
            {
                actionStep.IsDeleted = true;
                actionStep.DeletedBy = personId;
                actionStep.DeletedOn = DateTime.Now;
            }
            await _actionRepo.Update(action);
            return new BaseResponse
            {
                Status = true,
                Message = "Action Activated!",
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable to Activate Action!"
        };
    }
    public async Task TriggerActionsForExternal(int externalId)
    {
        var external = await _externalRepo.Get(x => x.Id == externalId);
        if(external != null)
        {
            foreach(var action in external.ActionIds)
            {
                await TriggerActions(action);
            }
        }
    }
    private async Task TriggerActions(int actionId)
    {
        var action = await _actionRepo.GetById(actionId);
        if (action != null)
        {
            var log = new CreateLogDto();
            foreach(var actionStep in action.ActionSteps)
            {
                if(actionStep.FacilityType == FacilityType.Appliance)
                {
                    var appliance = new UpdateApplianceDto();
                    appliance.Id = actionStep.FacilityTypeId;
                    appliance.IsActive = actionStep.IsActive;
                    await _applianceService.UpdateAppliance(appliance, action.PersonId);

                    log.PersonId = action.PersonId;
                    log.LogDetails = "Appliance Triggered by Sersor or User";
                    log.ActionType = "Appliance Activated"; 
                    await _logService.CreateLog(log);
                }
                if(actionStep.FacilityType == FacilityType.Camera)
                {
                    var camera = new UpdateCameraDto();
                    camera.Id = actionStep.FacilityTypeId;
                    camera.IsActive = actionStep.IsActive;
                    await _cameraService.UpdateCamera(camera, action.PersonId);

                    log.PersonId = action.PersonId;
                    log.LogDetails = "Camera Triggered by Sersor or User";
                    log.ActionType = "Camera Activated"; 
                    await _logService.CreateLog(log);
                }
                if(actionStep.FacilityType == FacilityType.Light)
                {
                    var light = new UpdateLightDto();
                    light.Id = actionStep.FacilityTypeId;
                    light.BrightnessLevel = actionStep.BrightnessLevel;
                    light.IsActive = actionStep.IsActive;
                    await _lightService.UpdateLight(light, action.PersonId);

                    log.PersonId = action.PersonId;
                    log.LogDetails = "Light Triggered by Sersor or User";
                    log.ActionType = "Light Activated"; 
                    await _logService.CreateLog(log);
                }
                if(actionStep.FacilityType == FacilityType.Door)
                {
                    var door = new UpdateDoorDto();
                    door.Id = actionStep.FacilityTypeId;
                    door.IsOpen = actionStep.IsOpen;
                    door.IsLocked = actionStep.IsLocked;
                    await _doorService.UpdateDoor(door, action.PersonId);

                    log.PersonId = action.PersonId;
                    log.LogDetails = "Door Triggered by Sersor or User";
                    log.ActionType = "Door Activated"; 
                    await _logService.CreateLog(log);
                }
                if(actionStep.FacilityType == FacilityType.Window)
                {
                    var window = new UpdateWindowDto();
                    window.Id = actionStep.FacilityTypeId;
                    window.IsOpen = actionStep.IsOpen;
                    window.IsLocked = actionStep.IsLocked;
                    await _windowService.UpdateWindow(window, action.PersonId);
                    
                    log.PersonId = action.PersonId;
                    log.LogDetails = "Window Triggered by Sersor or User";
                    log.ActionType = "Window Activated"; 
                    await _logService.CreateLog(log);
                }
            }
            log.PersonId = action.PersonId;
            log.LogDetails = "Action Triggered by Sersor or User";
            log.ActionType = "Action Activated"; 
            await _logService.CreateLog(log);
        }
    }
    public async Task<BaseResponse> AddActionStep(List<CreateActionStepDto> createActionStepDtos, int personId)
    {
        var action = await _actionRepo.GetById(createActionStepDtos.FirstOrDefault().ActionId);
        if (action != null)
        {
            foreach(var actionStep in createActionStepDtos)
            {
                var step = new ActionStep
                {
                    ActionId = actionStep.ActionId,
                    BrightnessLevel = actionStep.BrightnessLevel,
                    FacilityType = actionStep.FacilityType,
                    FacilityTypeId = actionStep.FacilityTypeId,
                    IsActive = actionStep.IsActive,
                    IsLocked = actionStep.IsLocked,
                    IsOpen = actionStep.IsOpen,
                    IsDeleted = false,
                    CreatedBy = personId,
                    LastModifiedBy = personId,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                };
                action.ActionSteps.Add(step);
            }
            await _actionRepo.Update(action);
            return new BaseResponse
            {
                Status = true,
                Message = "Action Step Added!",
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable to Add Action Step!"
        };
    }

    public async Task<BaseResponse> UpdateActionStep(List<UpdateActionStepDto> updateActionStepDtos, int personId)
    {
        var action = await _actionRepo.GetById(updateActionStepDtos.FirstOrDefault().ActionId);
        if (action != null)
        {
            foreach(var actionStep in updateActionStepDtos)
            {
                var step = action.ActionSteps.FirstOrDefault(x => x.Id == actionStep.Id);
                if(step != null)
                {
                    step.BrightnessLevel = actionStep.BrightnessLevel;
                    step.FacilityType = actionStep.FacilityType;
                    step.FacilityTypeId = actionStep.FacilityTypeId;
                    step.IsActive = actionStep.IsActive;
                    step.IsLocked = actionStep.IsLocked;
                    step.IsOpen = actionStep.IsOpen;
                    step.LastModifiedOn = DateTime.Now;
                    step.LastModifiedBy = personId;
                }
            }
            await _actionRepo.Update(action);
            return new BaseResponse
            {
                Status = true,
                Message = "Action Step Updated!",
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable to Update Action Step!"
        };
    }
    public async Task<BaseResponse> DeleteActionStep(int actionId, int actionStepId, int personId)
    {
        var action = await _actionRepo.GetById(actionId);
        if (action != null)
        {
            var actionStep = action.ActionSteps.FirstOrDefault(x => x.Id == actionStepId);
            if(actionStep != null)
            {
                actionStep.IsDeleted = true;
                actionStep.DeletedBy = personId;
                actionStep.DeletedOn = DateTime.Now;
                await _actionRepo.Update(action);
                return new BaseResponse
                {
                    Status = true,
                    Message = "Action Step Deleted!",
                };
            }
            return new BaseResponse
            {
                Status = true,
                Message = "Unable to find Action Step!",
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable to Delete Action Step!"
        };
    }
    private GetActionDto GetDetails(Action action)
    {
        return new GetActionDto
        {
            Id = action.Id,
            ActionName = action.ActionName,
            ActionDescription = action.Description,
            ActionActive = action.IsActive,
            ActionId = action.ActionId,
            GetActionStepDtos = action.ActionSteps.Select(x => new GetActionStepDto
            {
                Id = x.Id,
                ActionId = x.ActionId,
                FacilityType = x.FacilityType,
                FacilityTypeId = x.FacilityTypeId,
                BrightnessLevel = x.BrightnessLevel,
                IsLocked = x.IsLocked,
                IsOpen = x.IsOpen,
                IsActive = x.IsActive,
            }).ToList(),
        };
    }
}