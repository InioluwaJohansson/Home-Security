using Home_Security.Contracts;
using Home_Security.Entities.Identity;
namespace Home_Security.Entities;
public class Recording : AuditableEntity
{
    public int CameraId { get; set; }
    public Camera Camera { get; set; }
    public string FilePath { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public long FileSize { get; set; }
}