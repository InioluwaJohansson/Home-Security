using Home_Security.Contracts;
using Home_Security.Entities;

namespace Home_Security;

/// <summary>
/// Places an existing Room on a floor. SpatialFloorSectionId is optional because
/// rooms are allowed to belong directly to a floor without belonging to a section.
/// </summary>
public class SpatialRoomBinding : AuditableEntity
{
    public int SpatialFloorId { get; set; }
    public int RoomId { get; set; }
    public int? SpatialFloorSectionId { get; set; }
    public int? GeometryId { get; set; }

    public SpatialFloor? Floor { get; set; }
    public Room? Room { get; set; }
    public SpatialFloorSection? FloorSection { get; set; }
    public SpatialGeometry? Geometry { get; set; }
}
