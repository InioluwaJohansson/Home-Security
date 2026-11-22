using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// A storey in a building. Elevation is measured in the project's common spatial units.
/// Floor surfaces are linked separately so one physical slab can be both a lower
/// floor's ceiling and the upper floor's ground surface.
/// </summary>
public class SpatialFloor : AuditableEntity
{
    public int SpatialBuildingId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int LevelIndex { get; set; }
    public double Elevation { get; set; }
    public double? CeilingElevation { get; set; }
    public double? DefaultWallHeight { get; set; }
    public int? FootprintGeometryId { get; set; }

    public SpatialBuilding? Building { get; set; }
    public SpatialGeometry? FootprintGeometry { get; set; }
    public ICollection<SpatialFloorSection> Sections { get; set; } = new List<SpatialFloorSection>();
    public ICollection<SpatialRoomBinding> Rooms { get; set; } = new List<SpatialRoomBinding>();
    public ICollection<SpatialFloorSurfaceAssignment> SurfaceAssignments { get; set; } = new List<SpatialFloorSurfaceAssignment>();
    public ICollection<SpatialWallFloor> WallFloors { get; set; } = new List<SpatialWallFloor>();
}
