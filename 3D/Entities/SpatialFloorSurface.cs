using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// A physical horizontal surface/slab. It can be assigned as the ceiling of one
/// floor and the ground of another floor through multiple assignments.
/// </summary>
public class SpatialFloorSurface : AuditableEntity
{
    public int SpatialBuildingId { get; set; }
    public string? Name { get; set; }
    public SpatialSurfaceType SurfaceType { get; set; }
    public double Thickness { get; set; }
    public int? GeometryId { get; set; }

    public SpatialBuilding? Building { get; set; }
    public SpatialGeometry? Geometry { get; set; }
    public ICollection<SpatialFloorSurfaceAssignment> FloorAssignments { get; set; } = new List<SpatialFloorSurfaceAssignment>();
}
