using Home_Security.Contracts;
namespace Home_Security.Entities;
public class Room : AuditableEntity
{
    public string RoomName { get; set; }
    public string RoomId { get; set; }
    public Person Person { get; set; }
    public int PersonId { get; set; }
    public int SectionId { get; set; }
    public List<Door> Door { get; set; }
    public List<Light> Light { get; set; }
    public List<Window> Window { get; set; }
    public List<Appliance> Appliance { get; set; }
}
