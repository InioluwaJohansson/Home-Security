using Home_Security.Contracts;
namespace Home_Security.Entities;
public class ContactCategory : AuditableEntity
{
    public string CategoryId { get; set; }
    public int PersonId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
