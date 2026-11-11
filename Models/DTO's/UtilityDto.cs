using Home_Security.Entities;
using Home_Security.Models.Enums;
namespace Home_Security.Models.DTOs;
public class CreateUtilityDto
{
    //General Utilities
    public string UtilityName { get; set; }
    public UtilityType UtilityType { get; set; }
    public UtilityComponent? UtilityComponent { get; set; }
    
    //For Electricity & HVAC
    public int? CurrentPowerLevel { get; set; }
    public int? VoltageLevel { get; set; }
    public int? PowerConsumption { get; set; }
    public int? PowerConsumptionToday { get; set; }

    //For Emergencies
    public List<int>? ActionIds { get; set; } = new List<int>();
    public int? EmergencyTimeOut { get; set; }

    // For Water
    public int? CurrentWaterLevel { get; set; }
    public int? MaxWaterLevel { get; set; }
    public int? MinWaterLevel { get; set; }

    // For Irrigation
    public int? IrrigationDuration { get; set; }
}
public class UpdateUtilityDto
{
    //General Utilities
    public int Id { get; set; }
    public string UtilityName { get; set; }
    public string UtilityId { get; set; }
    public UtilityType UtilityType { get; set; }
    public UtilityComponent? UtilityComponent { get; set; }
    public bool IsActive { get; set; }
    
    //For Electricity & HVAC
    public double? CurrentPowerLevel { get; set; }
    public double? VoltageLevel { get; set; }
    public double? PowerConsumption { get; set; }
    public double? PowerConsumptionToday { get; set; }

    //For Emergencies
    public List<int>? ActionIds { get; set; } = new List<int>();
    public int? EmergencyTimeOut { get; set; }

    // For Water
    public double? CurrentWaterLevel { get; set; }
    public double? MaxWaterLevel { get; set; }
    public double? MinWaterLevel { get; set; }

    // For Irrigation
    public int? IrrigationDuration { get; set; }
}
public class GetUtilityDto
{
    //General Utilities
    public int PersonId { get; set; }
    public string UtilityName { get; set; }
    public string UtilityId { get; set; }
    public UtilityType UtilityType { get; set; }
    public UtilityComponent? UtilityComponent { get; set; }
    public bool IsActive { get; set; }
    
    //For Electricity & HVAC
    public double? CurrentPowerLevel { get; set; }
    public double? VoltageLevel { get; set; }
    public double? PowerConsumption { get; set; }
    public double? PowerConsumptionToday { get; set; }

    //For Emergencies
    public List<int>? ActionIds { get; set; } = new List<int>();
    public int? EmergencyTimeOut { get; set; }

    // For Water
    public double? CurrentWaterLevel { get; set; }
    public double? MaxWaterLevel { get; set; }
    public double? MinWaterLevel { get; set; }

    // For Irrigation
    public int? IrrigationDuration { get; set; }
}
public class UtilityResponseModel : BaseResponse
{
    public GetUtilityDto Data { get; set; }
}
public class UtilitiesResponseModel : BaseResponse
{
    public ICollection<GetUtilityDto> Data { get; set; } = new HashSet<GetUtilityDto>();
}