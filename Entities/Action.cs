using Home_Security.Contracts;
namespace Home_Security.Entities;
public class Action : AuditableEntity
{
    public string ActionId { get; set; }
    public string ActionName { get; set; }
    public TimeOnly? StartTIme { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string Description { get; set; }
    public TimeSpan? Duration { get; set; }
}