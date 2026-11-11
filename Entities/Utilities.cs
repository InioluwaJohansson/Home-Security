using Home_Security.Contracts;
using Home_Security.Models.Enums;
namespace Home_Security.Entities;
public class Utilities : AuditableEntity
{
    //General Utilities
    public int PersonId { get; set; }
    public string UtilityName { get; set; }
    public string UtilityId { get; set; }
    public UtilityType UtilityType { get; set; }
    public UtilityComponent? UtilityComponent { get; set; }
    public bool IsActive { get; set; }

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