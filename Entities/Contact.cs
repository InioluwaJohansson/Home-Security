using Home_Security.Contracts;
namespace Home_Security.Entities;
public class Contact : AuditableEntity
{
    public string ContactId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ImageUrl { get; set; }
    public ContactCategory Category { get; set; }
    public int ContactCategory { get; set; }
    public List<ContactDetails> ContactDetails { get; set; }
    public List<Address> Address { get; set; }
}

