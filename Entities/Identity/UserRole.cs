using Home_Security.Contracts;

namespace Home_Security.Entities.Identity;

public class UserRole : AuditableEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public Role Role { get; set; }
}
