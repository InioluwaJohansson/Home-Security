
using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Services;

public class UtilityUpdateService : IUtilityUpdateService
{
    private readonly IUtilityRepo _utilityRepo;
    private readonly IRealtimeNotificationService _realtime;
    private readonly IMqttService _mqtt;

    public UtilityUpdateService(IUtilityRepo utilityRepo, IRealtimeNotificationService realtime, IMqttService mqtt)
    {
        _utilityRepo = utilityRepo;
        _realtime = realtime;
        _mqtt = mqtt;
    }
    public async Task<BaseResponse> UpdateUtility(UpdateUtilityDto updateUtilityDto, int personId)
    {
        var utility = await _utilityRepo.Get(x => x.Id == updateUtilityDto.Id);
        if (utility != null)
        {
            utility.UtilityName = updateUtilityDto.UtilityName ?? utility.UtilityName;
            utility.UtilityType = updateUtilityDto.UtilityType;
            utility.UtilityComponent = updateUtilityDto.UtilityComponent ?? utility.UtilityComponent;
            utility.IsActive = updateUtilityDto.IsActive;
            utility.CurrentLevel = updateUtilityDto.CurrentLevel;
            utility.VoltageLevel = updateUtilityDto.VoltageLevel;
            utility.PowerConsumption = updateUtilityDto.PowerConsumption;
            utility.PowerConsumptionToday = updateUtilityDto.PowerConsumptionToday;
            utility.ActionIds = updateUtilityDto.ActionIds ?? utility.ActionIds;
            utility.TimeOut = updateUtilityDto.TimeOut;
            utility.CurrentWaterLevel = updateUtilityDto.CurrentWaterLevel;
            utility.MaxWaterLevel = updateUtilityDto.MaxWaterLevel;
            utility.MinWaterLevel = updateUtilityDto.MinWaterLevel;
            utility.IsDeleted = false;
            utility.LastModifiedBy = personId;
            utility.LastModifiedOn = DateTime.Now;

            await _utilityRepo.Update(utility);

            await _realtime.NotifyAll(
                "UtilityUpdated",
                new List<Role> { Role.Owner, Role.Wife, Role.Child },
                GetDetails(utility));

            await _mqtt.PublishUtilityUpdate(
                utility.UtilityId,
                new GenericUtilityUpdateState
                {
                    Id = utility.Id,
                    UtilityType = utility.UtilityType,
                    UtilityComponent = utility.UtilityComponent,
                    IsActive = utility.IsActive,
                    PowerConsumptionToday = utility.PowerConsumptionToday,
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
    public async Task<Utilities> GetById(int utilityId)
    {
        var utility = await _utilityRepo.Get(x => x.Id == utilityId);
        if (utility != null)
        {
            return utility;
        }
        return null;
    }
    private GetUtilityDto GetDetails(Utilities utility)
    {
        return new GetUtilityDto
        {
            Id = utility.Id,
            personId = utility.PersonId,
            UtilityName = utility.UtilityName,
            UtilityId = utility.UtilityId,
            UtilityType = utility.UtilityType,
            UtilityComponent = utility.UtilityComponent ?? 0,
            IsActive = utility.IsActive,
            CurrentLevel = utility.CurrentLevel,
            VoltageLevel = utility.VoltageLevel,
            PowerConsumption = utility.PowerConsumption,
            PowerConsumptionToday = utility.PowerConsumptionToday,
            ActionIds = utility.ActionIds,
            TimeOut = utility.TimeOut,
            CurrentWaterLevel = utility.CurrentWaterLevel,
            MaxWaterLevel = utility.MaxWaterLevel,
            MinWaterLevel = utility.MinWaterLevel,
            LastModifiedBy = utility.LastModifiedBy,
            DeletedBy = utility.DeletedBy,
            CreatedBy = utility.CreatedBy,
            LastModifiedOn = utility.LastModifiedOn,
            CreatedOn = utility.CreatedOn,
            IsDeleted = utility.IsDeleted
        };
    }
}
