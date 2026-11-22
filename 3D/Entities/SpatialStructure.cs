using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// Independent physical structures within the property, such as scaffolds.
/// They need not belong to a floor plan and can support other structures/equipment.
/// </summary>
public class SpatialStructure : AuditableEntity
{
    public int SpatialPropertyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public SpatialStructureType StructureType { get; set; }
    public int? SpatialBuildingId { get; set; }
    public int? SpatialFloorId { get; set; }
    public int? GeometryId { get; set; }
    public double? LoadCapacity { get; set; }
    public string? Description { get; set; }

    public SpatialProperty? Property { get; set; }
    public SpatialBuilding? Building { get; set; }
    public SpatialFloor? Floor { get; set; }
    public SpatialGeometry? Geometry { get; set; }
    public ICollection<SpatialStructureSupport> Supports { get; set; } = new List<SpatialStructureSupport>();
    public ICollection<SpatialStructureSupport> SupportedBy { get; set; } = new List<SpatialStructureSupport>();
}
