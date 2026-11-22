using Home_Security.Contracts;

namespace Home_Security;

/// <summary>A stair system connecting floors; flights are straight and may turn at landings.</summary>
public class SpatialStaircase : AuditableEntity
{
    public int SpatialBuildingId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int FromFloorId { get; set; }
    public int ToFloorId { get; set; }
    public int? GeometryId { get; set; }

    public SpatialBuilding? Building { get; set; }
    public SpatialGeometry? Geometry { get; set; }
    public ICollection<SpatialStairFlight> Flights { get; set; } = new List<SpatialStairFlight>();
    public ICollection<SpatialStairLanding> Landings { get; set; } = new List<SpatialStairLanding>();
}
