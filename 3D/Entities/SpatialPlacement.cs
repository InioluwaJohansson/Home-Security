using Home_Security.Contracts;
using Home_Security.Models.Enums;

namespace Home_Security;

/// <summary>
/// Spatial position for an existing smart-home facility. FacilityType + FacilityId
/// points to the existing authoritative record; it does not copy device state.
/// Cameras may anchor to Wall or FloorSurface (ceiling mounting is represented by
/// a FloorSurface assigned the Ceiling role). AnchorType + AnchorId is polymorphic.
/// </summary>
public class SpatialPlacement : AuditableEntity
{
    public FacilityType FacilityType { get; set; }
    public int FacilityId { get; set; }
    public SpatialAnchorType AnchorType { get; set; }
    public int AnchorId { get; set; }
    public SpatialMountingType MountingType { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public double PositionZ { get; set; }
    public double RotationX { get; set; }
    public double RotationY { get; set; }
    public double RotationZ { get; set; }
    public double ScaleX { get; set; } = 1;
    public double ScaleY { get; set; } = 1;
    public double ScaleZ { get; set; } = 1;
    public int? GeometryId { get; set; }
    public string? MountingNotes { get; set; }

    public SpatialGeometry? Geometry { get; set; }
}
