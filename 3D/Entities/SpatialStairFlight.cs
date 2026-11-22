using Home_Security.Contracts;

namespace Home_Security;

/// <summary>A straight flight between landings or floors.</summary>
public class SpatialStairFlight : AuditableEntity
{
    public int SpatialStaircaseId { get; set; }
    public int Sequence { get; set; }
    public int? FromLandingId { get; set; }
    public int? ToLandingId { get; set; }
    public int? FromFloorId { get; set; }
    public int? ToFloorId { get; set; }
    public int StepCount { get; set; }
    public double Width { get; set; }
    public double RiserHeight { get; set; }
    public double TreadDepth { get; set; }
    public int? GeometryId { get; set; }

    public SpatialStaircase? Staircase { get; set; }
    public SpatialGeometry? Geometry { get; set; }
}
