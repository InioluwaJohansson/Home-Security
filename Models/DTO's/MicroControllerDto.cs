
﻿namespace Home_Security.Models.DTOs;
public class GenericDeviceState
{
    public int Id { get; set; }

    public bool IsOpen { get; set; }

    public bool PowerActive { get; set; }

    public string DeviceId { get; set; } = default!;

    public long Timestamp { get; set; }
}
