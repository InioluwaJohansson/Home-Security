using Home_Security.Contracts;
using Home_Security.Models.Enums;
namespace Home_Security.Entities;
public class ActionStep : AuditableEntity
{
    public int ActionId  { get; set; }
    public FacilityType FacilityType { get; set; }
    public int FacilityTypeId { get; set; }
    public int BrightnessLevel { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public bool IsActive { get; set; }
}