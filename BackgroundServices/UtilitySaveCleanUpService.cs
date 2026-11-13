using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;
using Microsoft.Extensions.Hosting;
namespace Home_Security.BackgroundServices;

public class UtilitySaveCleanUpService : IUtilitySaveCleanUpService
{
    public IAuthCache _authCache;
    public IUserRepo _userRepo;
    public IUtilityRepo _utiltyRepo;
    public IUtilityService _utiltyService;
    public IRealtimeNotificationService _realtimeNotificationService;
    public UtilitySaveCleanUpService(IAuthCache authCache, IUserRepo userRepo, IUtilityRepo utilityRepo, IUtilityService utilityService, IRealtimeNotificationService realtimeNotificationService)
    {
        _authCache = authCache;
        _userRepo = userRepo;
        _utiltyRepo = utilityRepo;
        _utiltyService = utilityService;
        _realtimeNotificationService = realtimeNotificationService;
    }
    public async Task ExecuteCleanUp(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SaveUtilityData();
                _authCache._utilityStateCache.Clear();
            }
            catch { }
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
    public async Task ExecuteUtilityCheck(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var utilities = await _utiltyRepo.GetByExpression(x => x.IsDeleted == false && x.IsActive == true);
                if (utilities != null)
                {
                    foreach (var x in utilities)
                    {
                        if (x.LastModifiedOn?.AddMinutes(x.TimeOut ?? 0) < DateTime.Now && x.TimeOut.HasValue)
                        {
                            x.IsActive = false;
                            await UpdateUtility(x); 
                        }
                    }
                }
            }
            catch { }
            await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
        }
    }
    public async Task SaveUtilityCacheData(UtilityDataMQTT utilityDataMQTT)
    {
        _authCache._utilityStateCache.Add(utilityDataMQTT);
        await _realtimeNotificationService.NotifyAll("UtilityUpdated", new List<Role> { Role.Owner, Role.Wife, Role.Child }, utilityDataMQTT);
    }
    public async Task SaveUtilityData()
    {
        var utilities = await _utiltyRepo.GetByExpression(x => x.IsDeleted == false);
        if (utilities != null)
        {
            foreach (var uMQTT in _authCache._utilityStateCache)
            {
                var utility = utilities.FirstOrDefault(x => x.Id == uMQTT.Id);
                if (utility != null)
                {
                    utilities.FirstOrDefault(x => x.Id == uMQTT.Id).PowerConsumption += uMQTT.PowerConsumptionToday;
                    utilities.FirstOrDefault(x => x.Id == uMQTT.Id).PowerConsumptionToday += uMQTT.PowerConsumptionToday;
                    utilities.FirstOrDefault(x => x.Id == uMQTT.Id).CurrentWaterLevel = uMQTT.CurrentWaterLevel;
                    utilities.FirstOrDefault(x => x.Id == uMQTT.Id).CurrentLevel += uMQTT.CurrentLevel;
                    utilities.FirstOrDefault(x => x.Id == uMQTT.Id).VoltageLevel += uMQTT.VoltageLevel;
                }
            }
            foreach (var utility in utilities)
            {
                var uMQTTCount = _authCache._utilityStateCache.Where(x => x.Id == utility.Id).Count() + 1;
                utility.CurrentLevel = utility.CurrentLevel / uMQTTCount;
                utility.VoltageLevel = utility.VoltageLevel / uMQTTCount;
                await UpdateUtility(utility);       
            }
        }
    }
    public async Task UpdateUtility(Utilities utility)
    {
        var users = await _userRepo.GetAllUsers();
        if (users != null)
        {
            var updateUtilityDto = new UpdateUtilityDto()
            {
                Id = utility.Id,
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
            };
            var system = users.FirstOrDefault(x => x.UserRole.Role == Role.System);
            if (system != null)  await _utiltyService.UpdateUtility(updateUtilityDto, system.Id);
        }
    }
}