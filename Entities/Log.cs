using Home_Security.Contracts;
using Home_Security.Models.Enums;
namespace Home_Security.Entities;
public class Logs: AuditableEntity
{
    public Person Person { get; set; }
    public int PersonId { get; set; }
    public string ActionType { get; set; }
    public DateTime TimeOfAction { get; set; }
    public string LogDetails { get; set; }
    public FacilityTypeApp FacilityType { get; set;}
    public int? FacilityId { get; set; }
}