using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// Gives a physical surface a role relative to a floor. The same SurfaceId may
/// have Ceiling on the lower floor and Ground on the floor immediately above.
/// </summary>
public class SpatialFloorSurfaceAssignment : AuditableEntity
{
    public int SpatialFloorId { get; set; }
    public int SpatialFloorSurfaceId { get; set; }
    public SpatialSurfaceRole Role { get; set; }

    public SpatialFloor? Floor { get; set; }
    public SpatialFloorSurface? Surface { get; set; }
}
