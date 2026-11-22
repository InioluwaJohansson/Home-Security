using Home_Security.Contracts;

namespace Home_Security;

/// <summary>An outdoor/site area inside the property boundary, such as a yard or driveway.</summary>
public class SpatialExteriorArea : AuditableEntity
{
    public int SpatialPropertyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? GeometryId { get; set; }

    public SpatialProperty? Property { get; set; }
    public SpatialGeometry? Geometry { get; set; }
}
