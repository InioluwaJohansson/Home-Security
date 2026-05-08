
﻿namespace Home_Security.Models.DTOs;
public class GenericDeviceState
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public bool PowerActive { get; set; }
    public int BrightnessLevel { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }

    public long Timestamp { get; set; }
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
