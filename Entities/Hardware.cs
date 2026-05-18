using Home_Security.Contracts;
using System.Dynamic;

namespace Home_Security.Entities;
public class Hardware : AuditableEntity
{
    public string HardwareName { get; set; }
    public string HardwareId { get; set; }
    public string AuthKey { get; set; }
    public bool IsActive { get; set; }
    public bool PowerActive { get; set; }
    public List<int> LightIds { get; set; } = new List<int>();
    public List<int> ApplianceIds { get; set; } = new List<int>();
    public List<int> CameraIds { get; set; } = new List<int>();
    public List<int> WindowIds { get; set; } = new List<int>();
    public List<int> DoorIds { get; set; } = new List<int>();
    public List<int> ExternalIds { get; set; } = new List<int>();
}