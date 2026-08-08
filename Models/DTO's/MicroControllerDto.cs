
using Home_Security.Models.Enums;

namespace Home_Security.Models.DTOs;
public class GenericDeviceState : BaseResponse
{
    public List<ApplianceDeviceState> ApplianceDeviceStates { get; set; }
    public List<LightDeviceState> LightDeviceStates { get; set; }
    public List<CameraDeviceState> CameraDeviceStates { get; set; }
    public List<WindowDeviceState> WindowDeviceStates { get; set; }
    public List<DoorDeviceState> DoorDeviceStates { get; set; }
    public List<ExternalDeviceState> ExternalDeviceStates { get; set; }
    public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    public string Token { get; set;}
}
public class GenericDeviceUpdateState
{
    public FacilityType FacilityType { get; set; }
    public int Id { get; set; }
    public int BrightnessLevel { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public bool IsTriggered { get; set; }
    public bool IsActive { get; set; }
    public bool PowerActive { get; set; }
    public string Token { get; set; }
}
public class ApplianceDeviceState
{
    public int ApplianceId { get; set; }
    public bool IsActive { get; set; }
    public ApplianceType ApplianceType { get; set; }
}
public class LightDeviceState
{
    public int LightId { get; set; }
    public int BrightnessLevel { get; set; }
    public bool IsActive { get; set; }
}
public class CameraDeviceState
{
    public int CameraId { get; set; }
    public bool IsActive { get; set; }
}
public class WindowDeviceState
{
    public int WindowId { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
}
public class DoorDeviceState
{
    public int DoorId { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public DoorType DoorType { get; set; }
}
public class ExternalDeviceState
{
    public int ExternalId { get; set; }
    public bool IsActive { get; set; }
    public bool IsTriggered { get; set; }
}
public class MicroControllerLoginDto
{
    public string HardwareId { get; set; }
    public string AuthKey { get; set; }
}
public class MTTQConfig : BaseResponse
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}