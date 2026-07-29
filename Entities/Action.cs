using Home_Security.Contracts;
namespace Home_Security.Entities;
public class Action : AuditableEntity
{
    public string ActionId { get; set; }
    public string ActionName { get; set; }
    public string Description { get; set; }
    public bool IsPrivate { get; set; } = false;
    public bool IsRecurring { get; set; } = false;
    public TimeSpan? Time { get; set; } 
    public DateTime LastRunDate { get; set; }
    public int PersonId { get; set; }
    public bool IsActive { get; set; }
    public List<ActionStep> ActionSteps{ get; set; }
}