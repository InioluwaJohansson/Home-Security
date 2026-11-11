using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Services;
public class UtilityService : IUtilityService
{
    IUtilityRepo _UtilityRepo;
    IHardwareService _hardwareService;
    IRealtimeNotificationService _realtime;
    IMqttService _mqtt;
    public UtilityService(IUtilityRepo UtilityRepo, IHardwareService hardwareService, IRealtimeNotificationService realtime, IMqttService mqtt)
    {
        _UtilityRepo = UtilityRepo;
        _hardwareService = hardwareService;
        _realtime = realtime;
        _mqtt = mqtt;
    }
    public async Task<BaseResponse> CreateUtility(CreateUtilityDto createUtilityDto, int personId)
    {
        if (createUtilityDto != null)
        {   
            var utility = new Utilities
            {
                PersonId = personId,
                UtilityName = createUtilityDto.UtilityName,
                UtilityId = $"UTILITY{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                UtilityType = createUtilityDto.UtilityType,
                UtilityComponent = createUtilityDto.UtilityComponent,
                IsActive = false,
                CurrentPowerLevel = createUtilityDto.CurrentPowerLevel,
                VoltageLevel = createUtilityDto.VoltageLevel,
                PowerConsumption = createUtilityDto.PowerConsumption,
                PowerConsumptionToday = createUtilityDto.PowerConsumptionToday,
                ActionIds = createUtilityDto.ActionIds,
                EmergencyTimeOut = createUtilityDto.EmergencyTimeOut,
                CurrentWaterLevel = createUtilityDto.CurrentWaterLevel,
                MaxWaterLevel = createUtilityDto.MaxWaterLevel,
                MinWaterLevel = createUtilityDto.MinWaterLevel,
                IrrigationDuration = createUtilityDto.IrrigationDuration,
                IsDeleted = false,
                CreatedBy = personId,
                LastModifiedBy = personId,
                LastModifiedOn = DateTime.Now
            };

            utility = await _UtilityRepo.Create(utility);

            await _realtime.NotifyAll("UtilityCreated", new List<Role> { Role.Owner, Role.Wife, Role.Child }, GetDetails(utility));
            return new BaseResponse
            {
                Status = true,
                Message = "Utility created successfully"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Unable to create utility."
        };
    }
    public async Task<BaseResponse> UpdateUtility(UpdateUtilityDto updateUtilityDto, int personId)
    {
        var utility = await _UtilityRepo.Get(x => x.Id == updateUtilityDto.Id);
        if (utility != null)
        {
            utility.UtilityName = updateUtilityDto.UtilityName;
            utility.UtilityType = updateUtilityDto.UtilityType;
            utility.UtilityComponent = updateUtilityDto.UtilityComponent;
            utility.IsActive = updateUtilityDto.IsActive;
            utility.CurrentPowerLevel = updateUtilityDto.CurrentPowerLevel;
            utility.VoltageLevel = updateUtilityDto.VoltageLevel;
            utility.PowerConsumption = updateUtilityDto.PowerConsumption;
            utility.PowerConsumptionToday = updateUtilityDto.PowerConsumptionToday;
            utility.ActionIds = updateUtilityDto.ActionIds;
            utility.EmergencyTimeOut = updateUtilityDto.EmergencyTimeOut;
            utility.CurrentWaterLevel = updateUtilityDto.CurrentWaterLevel;
            utility.MaxWaterLevel = updateUtilityDto.MaxWaterLevel;
            utility.MinWaterLevel = updateUtilityDto.MinWaterLevel;
            utility.IrrigationDuration = updateUtilityDto.IrrigationDuration;
            utility.IsDeleted = false;
            utility.LastModifiedBy = personId;
            utility.LastModifiedOn = DateTime.Now;

            await _UtilityRepo.Update(utility);
            await _realtime.NotifyAll("UtilityUpdated", new List<Role> { Role.Owner, Role.Wife, Role.Child }, GetDetails(utility));
            await _mqtt.PublishUtilityUpdate(utility.UtilityId, new GenericUtilityUpdateState
            {
                Id = utility.Id,
                UtilityType = utility.UtilityType,
                UtilityComponent = utility.UtilityComponent,
                IsActive = utility.IsActive,
                CurrentPowerLevel = utility.CurrentPowerLevel,
                VoltageLevel = utility.VoltageLevel,
                PowerConsumptionToday = utility.PowerConsumptionToday,
                CurrentWaterLevel = utility.CurrentWaterLevel,
                MaxWaterLevel = utility.MaxWaterLevel,
                MinWaterLevel = utility.MinWaterLevel
            });
            return new BaseResponse
            {
                Status = true,
                Message = "Utility updated successfully"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Utility not found"
        };
    }
    public async Task<UtilityResponseModel> GetById(int utilityId)
    {
        var utility = await _UtilityRepo.Get(x => x.Id == utilityId);
        if (utility != null)
        {
            return new UtilityResponseModel
            {
                Data = GetDetails(utility),
                Message = "Utility retrieved successfully",
                Status = true
            };
        }
        return new UtilityResponseModel
        {
            Data = null,
            Message = "Utility not found",
            Status = false
        };
    }
    public async Task<UtilitiesResponseModel> GetAllUtilities()
    {
        var utilities = await _UtilityRepo.GetByExpression(x => x.IsDeleted == false);
        if (utilities != null && utilities.Any())
        {
            return new UtilitiesResponseModel
            {
                Data = utilities.Select(utility => GetDetails(utility)).ToList(),
                Message = "Utilities retrieved successfully",
                Status = true
            };
        }
        return new UtilitiesResponseModel
        {
            Data = null,
            Message = "No utilities found",
            Status = false
        };
    }
    public async Task<BaseResponse> Delete(int utilityId, int personId)
    {
        var utility = await _UtilityRepo.Get(x => x.Id == utilityId);
        if (utility != null)
        {
            utility.IsDeleted = true;
            utility.IsActive = false;
            utility.DeletedBy = personId;
            utility.DeletedOn = DateTime.Now;
            utility.LastModifiedBy = personId;
            utility.LastModifiedOn = DateTime.Now;
            await _UtilityRepo.Update(utility);
            await _realtime.NotifyAll("UtilityDeleted", new
            {
                utility.Id,
                utility.UtilityId
            });
            await _mqtt.PublishUtilityUpdate(utility.UtilityId, new GenericUtilityUpdateState
            {
                Id = utility.Id,
                UtilityType = 0,
                UtilityComponent = 0,
                IsActive = false,
                CurrentPowerLevel = 0,
                VoltageLevel = 0,
                PowerConsumptionToday = 0,
                CurrentWaterLevel = 0,
                MaxWaterLevel = 0,
                MinWaterLevel = 0
            });
            return new BaseResponse
            {
                Status = true,
                Message = "Utility deleted successfully"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Utility not found"
        };
    }
    public GetUtilityDto GetDetails(Utilities utility)
    {
        return new GetUtilityDto
        {
            PersonId = utility.PersonId,
            UtilityName = utility.UtilityName,
            UtilityId = utility.UtilityId,
            UtilityType = utility.UtilityType,
            UtilityComponent = utility.UtilityComponent ?? 0,
            IsActive = utility.IsActive,
            CurrentPowerLevel = utility.CurrentPowerLevel,
            VoltageLevel = utility.VoltageLevel,
            PowerConsumption = utility.PowerConsumption,
            PowerConsumptionToday = utility.PowerConsumptionToday,
            ActionIds = utility.ActionIds,
            EmergencyTimeOut = utility.EmergencyTimeOut,
            CurrentWaterLevel = utility.CurrentWaterLevel,
            MaxWaterLevel = utility.MaxWaterLevel,
            MinWaterLevel = utility.MinWaterLevel,
            IrrigationDuration = utility.IrrigationDuration
        };
    }
}