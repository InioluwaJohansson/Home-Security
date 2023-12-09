using Home_Security.Contracts;
namespace Home_Security.Entities;
public class BaseDefault : AuditableEntity
{
    public bool IsActive { get; set; }
    public bool PowerActive { get; set; }
    public int RoomId { get; set; }
    public int SectionId { get; set; }
}
