using Home_Security.Contracts;

namespace Home_Security;

/// <summary>
/// Optional per-person spatial visibility grant/override. Existing Room.IsHidden
/// and Section.IsHidden remain the source flags; this entity can express who may
/// access a hidden space. Actual filtering must be enforced server-side.
/// </summary>
public class SpatialAccessRule : AuditableEntity
{
    public int PersonId { get; set; }
    public SpatialGeometryOwnerType SpaceType { get; set; }
    public int SpaceId { get; set; }
    public bool CanView { get; set; }
    public bool CanControl { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
}
