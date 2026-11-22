using Home_Security.Contracts;

namespace Home_Security;

/// <summary>Associates a wall with every floor it physically spans.</summary>
public class SpatialWallFloor : AuditableEntity
{
    public int SpatialWallId { get; set; }
    public int SpatialFloorId { get; set; }

    public SpatialWall? Wall { get; set; }
    public SpatialFloor? Floor { get; set; }
}
