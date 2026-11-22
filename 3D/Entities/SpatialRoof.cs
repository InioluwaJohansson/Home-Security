using Home_Security.Contracts;

namespace Home_Security;

/// <summary>Building roof model; supports a flat roof with slanted/beveled perimeter edges.</summary>
public class SpatialRoof : AuditableEntity
{
    public int SpatialBuildingId { get; set; }
    public SpatialRoofType RoofType { get; set; } = SpatialRoofType.Flat;
    public string? Name { get; set; }
    public double? BaseElevation { get; set; }
    public int? GeometryId { get; set; }

    public SpatialBuilding? Building { get; set; }
    public SpatialGeometry? Geometry { get; set; }
    public ICollection<SpatialRoofEdge> Edges { get; set; } = new List<SpatialRoofEdge>();
}
