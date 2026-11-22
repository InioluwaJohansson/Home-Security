using Home_Security.Contracts;
using Home_Security.Entities;

namespace Home_Security;

/// <summary>
/// Spatial location for an existing Utilities record. There is intentionally no
/// Property anchor option: a utility cannot be attached directly to the property.
/// Irrigation is located within the property by anchoring it to an actual area,
/// floor, room, section, surface, wall, or structure.
/// </summary>
public class SpatialUtilityPlacement : AuditableEntity
{
    public int UtilityId { get; set; }
    public SpatialUtilityAnchorType AnchorType { get; set; }
    public int AnchorId { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public double PositionZ { get; set; }
    public double RotationX { get; set; }
    public double RotationY { get; set; }
    public double RotationZ { get; set; }
    public int? GeometryId { get; set; }
    public string? PlacementNotes { get; set; }

    public SpatialGeometry? Geometry { get; set; }
    public Utilities? Utility { get; set; }
}
