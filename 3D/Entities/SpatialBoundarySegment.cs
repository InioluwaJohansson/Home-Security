using Home_Security.Contracts;

namespace Home_Security;

/// <summary>One edge of the property perimeter; openings such as gates can be placed on it.</summary>
public class SpatialBoundarySegment : AuditableEntity
{
    public int SpatialPropertyId { get; set; }
    public int Sequence { get; set; }
    public int? GeometryId { get; set; }
    public double Height { get; set; }
    public double Thickness { get; set; }
    public string? Material { get; set; }

    public SpatialProperty? Property { get; set; }
    public SpatialGeometry? Geometry { get; set; }
    public ICollection<SpatialOpening> Openings { get; set; } = new List<SpatialOpening>();
}
