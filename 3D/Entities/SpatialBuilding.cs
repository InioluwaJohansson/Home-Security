using Home_Security.Contracts;

namespace Home_Security;

public class SpatialBuilding : AuditableEntity
{
    public int SpatialPropertyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? FootprintGeometryId { get; set; }

    public SpatialProperty? Property { get; set; }
    public SpatialGeometry? FootprintGeometry { get; set; }
    public ICollection<SpatialFloor> Floors { get; set; } = new List<SpatialFloor>();
    public SpatialRoof? Roof { get; set; }
}
