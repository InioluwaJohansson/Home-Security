namespace Home_Security.Entities;
public class Window : BaseDefault
{
    public string WindowName { get; set; }
    public string WindowId { get; set; }
    public bool IsOpen { get; set; }
    public bool IsLocked { get; set; }
    public int OpenedBy { get; set; }
    public int LockedBy { get; set; }
    public int UnlockedBy { get; set; }
}
