using Home_Security.Contracts;

namespace Home_Security;

/// <summary>Describes each roof perimeter edge, including slanted and beveled edges.</summary>
public class SpatialRoofEdge : AuditableEntity
{
    public int SpatialRoofId { get; set; }
    public int Sequence { get; set; }
    public SpatialRoofEdgeType EdgeType { get; set; }
    public double? HorizontalOffset { get; set; }
    public double? VerticalOffset { get; set; }
    public double? AngleDegrees { get; set; }
    public int? GeometryId { get; set; }

    public SpatialRoof? Roof { get; set; }
    public SpatialGeometry? Geometry { get; set; }
}
