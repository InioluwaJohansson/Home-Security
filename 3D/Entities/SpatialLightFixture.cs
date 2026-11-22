using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// A physical grouping/layout for one or more existing Light devices, such as
/// a square grid or a linear strip. Each member light retains its own state.
/// </summary>
public class SpatialLightFixture : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public int? SpatialFloorId { get; set; }
    public int? SpatialFloorSectionId { get; set; }
    public int? SpatialRoomBindingId { get; set; }
    public SpatialMountingType MountingType { get; set; }
    public int? GeometryId { get; set; }
    public string? LayoutDescription { get; set; }

    public SpatialFloor? Floor { get; set; }
    public SpatialFloorSection? FloorSection { get; set; }
    public SpatialRoomBinding? RoomBinding { get; set; }
    public SpatialGeometry? Geometry { get; set; }
    public ICollection<SpatialLightFixtureMember> Members { get; set; } = new List<SpatialLightFixtureMember>();
}
