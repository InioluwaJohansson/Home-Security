using Home_Security.Contracts;
namespace Home_Security.Entities;
public class Section : AuditableEntity
{
    public string SectionName { get; set;}
    public string SectionId { get; set; }
    public List<Room> Room { get; set;}
    public List<Door> Door { get; set;}
    public List<Light> Light { get; set;}
    public List<Window> Window { get; set;}
    public List<Appliance> Appliance { get; set;}
}