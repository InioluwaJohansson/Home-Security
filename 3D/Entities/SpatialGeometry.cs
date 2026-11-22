using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// Reusable geometric data. OwnerType + OwnerId identifies the spatial object
/// that owns this geometry; ownership is polymorphic and is validated by the service.
/// Coordinates use one consistent project-wide coordinate system and unit.
/// </summary>
public class SpatialGeometry : AuditableEntity
{
    public SpatialGeometryType GeometryType { get; set; }
    public SpatialGeometryOwnerType OwnerType { get; set; }
    public int OwnerId { get; set; }
    public string? Name { get; set; }
    public string? CoordinateSystem { get; set; }
    public double? MinX { get; set; }
    public double? MinY { get; set; }
    public double? MinZ { get; set; }
    public double? MaxX { get; set; }
    public double? MaxY { get; set; }
    public double? MaxZ { get; set; }

    public ICollection<SpatialGeometryPoint> Points { get; set; } = new List<SpatialGeometryPoint>();
    public ICollection<SpatialGeometrySpaceLink> SpaceLinks { get; set; } = new List<SpatialGeometrySpaceLink>();
}
