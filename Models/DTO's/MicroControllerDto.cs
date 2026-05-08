
﻿namespace Home_Security.Models.DTOs;
public class GenericDeviceState
{
    public List<ApplianceDeviceState> ApplianceDeviceStates { get; set; }
    public List<LightDeviceState> LightDeviceStates { get; set; }
    public List<CameraDeviceState> CameraDeviceStates { get; set; }
    public List<WindowDeviceState> WindowDeviceStates { get; set; }
    public List<DoorDeviceState> DoorDeviceStates { get; set; }
    public long Timestamp { get; set; } = DateTime.Now;
}
public class GenericDeviceUpdateState
{
    public int FacilityType { get; set; }
    public int Id { get; set; }
    public int BrightnessLevel { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public bool IsActive { get; set; }
    public bool PowerActive { get; set; }
}
public class ApplianceDeviceState
{
    public int ApplianceId { get; set; }
    public bool IsActive { get; set; }
    public bool PowerActive { get; set; }
}
public class LightDeviceState
{
    public int LightId { get; set; }
    public int BrightnessLevel { get; set; }
}
public class CameraDeviceState
{
    public int CameraId { get; set; }
    public bool IsActive { get; set; }
    public bool PowerActive { get; set; }
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
}
