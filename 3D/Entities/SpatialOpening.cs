using Home_Security.Contracts;
using Home_Security.Entities;

namespace Home_Security;

/// <summary>
/// A real void in a wall or property-boundary segment. Door/window records are
/// optional links to existing smart devices; the opening itself is spatial geometry.
/// A gate opening may be attached to a boundary segment instead of a wall.
/// </summary>
public class SpatialOpening : AuditableEntity
{
    public int? SpatialWallId { get; set; }
    public int? SpatialWallSegmentId { get; set; }
    public int? SpatialBoundarySegmentId { get; set; }
    public SpatialOpeningType OpeningType { get; set; }
    public int? DoorId { get; set; }
    public int? WindowId { get; set; }
    public double Offset { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double SillHeight { get; set; }
    public int? GeometryId { get; set; }

    public SpatialWall? Wall { get; set; }
    public SpatialWallSegment? WallSegment { get; set; }
    public SpatialBoundarySegment? BoundarySegment { get; set; }
    public Door? Door { get; set; }
    public Window? Window { get; set; }
    public SpatialGeometry? Geometry { get; set; }
}
