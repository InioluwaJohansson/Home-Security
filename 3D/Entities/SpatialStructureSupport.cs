using Home_Security.Contracts;

namespace Home_Security;

/// <summary>Represents one structure supporting another (for example, a scaffold supporting a tank).</summary>
public class SpatialStructureSupport : AuditableEntity
{
    public int SupportingStructureId { get; set; }
    public int SupportedStructureId { get; set; }
    public double? LoadShare { get; set; }
    public string? Notes { get; set; }

    public SpatialStructure? SupportingStructure { get; set; }
    public SpatialStructure? SupportedStructure { get; set; }
}
