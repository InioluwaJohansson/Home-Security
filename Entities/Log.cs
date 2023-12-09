using Home_Security.Contracts;
namespace Home_Security.Entities;
public class Logs: AuditableEntity
{
    public Person Person { get; set; }
    public int PersonId { get; set; }
    public string ActionType { get; set; }
    public DateTime TimeOfAction { get; set; }
    public string LogDetails { get; set; }
}