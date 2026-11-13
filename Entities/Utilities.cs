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
    public double? TimeOut { get; set; }

    //For Electricity & HVAC
    public double? CurrentLevel { get; set; }
    public double? VoltageLevel { get; set; }
    public double? PowerConsumption { get; set; }
    public double? PowerConsumptionToday { get; set; }

    //For Emergencies
    public List<int>? ActionIds { get; set; } = new List<int>();

    // For Water
    public double? CurrentWaterLevel { get; set; }
    public double? MaxWaterLevel { get; set; }
    public double? MinWaterLevel { get; set; }

}