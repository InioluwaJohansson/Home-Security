using Home_Security.Contracts;
using Home_Security.Entities;

namespace Home_Security;

/// <summary>Places an existing Section on a particular floor without duplicating the Section.</summary>
public class SpatialFloorSection : AuditableEntity
{
    public int SpatialFloorId { get; set; }
    public int SectionId { get; set; }
    public int? GeometryId { get; set; }

    public SpatialFloor? Floor { get; set; }
    public Section? Section { get; set; }
    public SpatialGeometry? Geometry { get; set; }
}
