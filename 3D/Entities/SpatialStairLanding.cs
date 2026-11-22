using Home_Security.Contracts;

namespace Home_Security;

/// <summary>A landing connects straight stair flights and permits direction changes.</summary>
public class SpatialStairLanding : AuditableEntity
{
    public int SpatialStaircaseId { get; set; }
    public int Sequence { get; set; }
    public double Width { get; set; }
    public double Length { get; set; }
    public double Elevation { get; set; }
    public int? GeometryId { get; set; }

    public SpatialStaircase? Staircase { get; set; }
    public SpatialGeometry? Geometry { get; set; }
}
