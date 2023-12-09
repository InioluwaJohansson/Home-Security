using Home_Security.Models.Enums;
using Home_Security.Contracts;
using Home_Security.Entities;
namespace Home_Security.Entities;
public class PersonDetails : AuditableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public string ImageUrl { get; set; } = "";
    public List<Address> Addresses { get; set; }
    public List<ContactDetails> ContactDetails { get; set; }
    public List<FingerPrint> FingerPrints { get; set; }
}
