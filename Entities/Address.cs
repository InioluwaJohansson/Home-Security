using Home_Security.Contracts;
namespace Home_Security.Entities;
public class Address: AuditableEntity
{
    public int ContactId { get; set; }
    public int PersonDetailsId { get; set; }
    public string NumberLine { get; set; } = "";
    public string Street { get; set; } = "";
    public string City { get; set; } = "";
    public string Region { get; set; } = "";
    public string State { get; set; } = "";
    public string Country { get; set; } = "";
    public string PostalCode { get; set; } = "";
}
