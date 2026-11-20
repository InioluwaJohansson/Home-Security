using Home_Security.Spatial.Entities;

namespace Home_Security.Spatial.Services;

public sealed record SpatialActor(int PersonId, int UserId, string RoleName);
public sealed record SpatialChange(int PropertyId, int ResourceId, string ResourceType, int Version, string Operation);
public interface ISpatialTargetValidator
{
    Task<bool> ExistsAndBelongsToPersonAsync(SpatialTargetKind kind, int targetId, int personId, CancellationToken ct);
}
public interface ISpatialGeometryValidator
{
    Task ValidatePlacementAsync(SpatialPlacement placement, CancellationToken ct);
}
public sealed class SpatialValidationException(string message) : Exception(message);
