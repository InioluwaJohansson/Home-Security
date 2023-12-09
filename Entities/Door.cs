using Home_Security.Models.Enums;
namespace Home_Security.Entities;
public class Door : BaseDefault
{
    public string DoorName { get; set; }
    public string DoorId { get; set; }
    public DoorType DoorType { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public int OpenedBy { get; set; }
    public int LockedBy { get; set; }
    public int UnlockedBy { get; set; }
}
