using Home_Security.Contracts;
using Home_Security.Entities;

namespace Home_Security;

/// <summary>Places one existing Light within a fixture/group layout.</summary>
public class SpatialLightFixtureMember : AuditableEntity
{
    public int SpatialLightFixtureId { get; set; }
    public int LightId { get; set; }
    public int Sequence { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public double PositionZ { get; set; }
    public double RotationX { get; set; }
    public double RotationY { get; set; }
    public double RotationZ { get; set; }

    public SpatialLightFixture? Fixture { get; set; }
    public Light? Light { get; set; }
}
