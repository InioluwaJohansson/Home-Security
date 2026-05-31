using Home_Security.Models.Enums;

namespace Home_Security.Entities;
public class Appliance : BaseDefault
{
    public string ApplianceName { get; set; }
    public string ApplianceId { get; set; }
    public ApplianceType ApplianceType { get; set; }
}
