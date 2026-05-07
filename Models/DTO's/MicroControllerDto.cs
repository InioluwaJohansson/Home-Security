
﻿namespace Home_Security.Models.DTOs;
public class DeviceStateMessage
{
    public string FacilityType { get; set; } = default!;

    public string FacilityId { get; set; } = default!;

    public bool State { get; set; }

    public string DeviceId { get; set; } = default!;

    public long Timestamp { get; set; }
}
