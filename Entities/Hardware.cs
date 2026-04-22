using Home_Security.Contracts;
using System.Dynamic;

namespace Home_Security.Entities;
public class Hardware : AuditableEntity
{
    public string HardwareName { get; set; }
    public string HardwareId { get; set; }
    public string AuthKey { get; set; }
    public List<int> LightIds { get; set; }
    public List<int> ApplianceIds { get; set; }
    public List<int> CameraIds { get; set; }
    public List<int> WindowIds { get; set; }
    public List<int> DoorIds { get; set; }
}