using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// Associates geometry with one or more spatial contexts (for example, a wall
/// crossing the underground and ground floors). SpaceType + SpaceId is polymorphic.
/// </summary>
public class SpatialGeometrySpaceLink : AuditableEntity
{
    public int SpatialGeometryId { get; set; }
    public SpatialGeometryOwnerType SpaceType { get; set; }
    public int SpaceId { get; set; }

    public SpatialGeometry? Geometry { get; set; }
}
