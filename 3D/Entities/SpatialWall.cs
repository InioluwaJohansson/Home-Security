using Home_Security.Contracts;

namespace Home_Security;

/// <summary>A physical wall. Floor membership is many-to-many so walls may span storeys.</summary>
public class SpatialWall : AuditableEntity
{
    public int SpatialBuildingId { get; set; }
    public string? Name { get; set; }
    public SpatialWallType WallType { get; set; }
    public double Thickness { get; set; }
    public string? Material { get; set; }
    public int? GeometryId { get; set; }

    public SpatialBuilding? Building { get; set; }
    public SpatialGeometry? Geometry { get; set; }
    public ICollection<SpatialWallSegment> Segments { get; set; } = new List<SpatialWallSegment>();
    public ICollection<SpatialOpening> Openings { get; set; } = new List<SpatialOpening>();
    public ICollection<SpatialWallFloor> Floors { get; set; } = new List<SpatialWallFloor>();
}
