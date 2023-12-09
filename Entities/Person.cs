using Home_Security.Contracts;
using Home_Security.Entities.Identity;
namespace Home_Security.Entities;
public class Person : AuditableEntity
{
    public string PersonId { get; set; }
    public PersonDetails PersonDetails { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public bool Disabled { get; set; }
}