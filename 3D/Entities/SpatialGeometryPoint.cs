using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// Ordered vertex/control point. For curved geometry, points can represent control
/// points or sampled points according to the owning geometry's GeometryType/metadata.
/// </summary>
public class SpatialGeometryPoint : AuditableEntity
{
    public int SpatialGeometryId { get; set; }
    public int Sequence { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double? Bulge { get; set; }

    public SpatialGeometry? Geometry { get; set; }
}
