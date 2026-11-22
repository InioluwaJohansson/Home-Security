using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// A wall may consist of multiple segments with different heights, profiles,
/// materials, or curved geometry.
/// </summary>
public class SpatialWallSegment : AuditableEntity
{
    public int SpatialWallId { get; set; }
    public int Sequence { get; set; }
    public int? GeometryId { get; set; }
    public double StartHeight { get; set; }
    public double EndHeight { get; set; }
    public double Thickness { get; set; }
    public string? Material { get; set; }
    public bool IsGlass { get; set; }

    public SpatialWall? Wall { get; set; }
    public SpatialGeometry? Geometry { get; set; }
}
