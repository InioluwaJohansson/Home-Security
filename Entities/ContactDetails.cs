using Home_Security.Contracts;
namespace Home_Security.Entities;
public class ContactDetails : AuditableEntity
{
    public int ContactId { get; set; }
    public int PersonDetailsId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
