using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// The site/property envelope. Its geometry describes the property boundary.
/// Individual utility placements deliberately cannot target this entity directly.
/// </summary>
public class SpatialProperty : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoordinateReferenceSystem { get; set; }
    public double? CoordinateUnitScale { get; set; }
    public int? BoundaryGeometryId { get; set; }

    public SpatialGeometry? BoundaryGeometry { get; set; }
    public ICollection<SpatialBuilding> Buildings { get; set; } = new List<SpatialBuilding>();
    public ICollection<SpatialExteriorArea> ExteriorAreas { get; set; } = new List<SpatialExteriorArea>();
    public ICollection<SpatialBoundarySegment> BoundarySegments { get; set; } = new List<SpatialBoundarySegment>();
    public ICollection<SpatialGeometry> Geometries { get; set; } = new List<SpatialGeometry>();
}
