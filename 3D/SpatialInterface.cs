using Home_Security.Spatial;

namespace Home_Security.Spatial;
public sealed record SpatialActor(int PersonId, int UserId, string RoleName);
public sealed record SpatialChange(int PropertyId, int ResourceId, string ResourceType, int Version, string Operation);
public interface ISpatialTargetValidator
{
    Task<bool> ExistsAndBelongsToPersonAsync(SpatialTargetKind kind, int targetId, int personId);
}
public interface ISpatialGeometryValidator
{
    Task ValidatePlacementAsync(SpatialPlacement placement);
}
public interface ISpatialService
{
    Task<List<SpatialProperty>> GetPropertiesAsync(SpatialActor actor);
    Task<SpatialProperty> CreatePropertyAsync(SpatialActor actor, string name);
    Task<SpatialBuilding> CreateBuildingAsync(SpatialActor actor, int propertyId, string name, double baseElevation);
    Task<SpatialFloor> CreateFloorAsync(SpatialActor actor, int buildingId, string name, int levelIndex, double elevation, double? ceilingHeight);
    Task BindSectionAsync(SpatialActor actor, int floorId, int sectionId);
    Task BindRoomAsync(SpatialActor actor, int floorId, int roomId, int? sectionId);
    Task<SpatialPlacement> CreatePlacementAsync(SpatialActor actor, SpatialPlacement p);
    Task DeletePlacementAsync(SpatialActor actor, int placementId);
    Task<SpatialGeometry> CreateGeometryAsync(SpatialActor actor, int propertyId, SpatialGeometryKind kind, string name, string geometryJson, double height, double thickness);
    Task<object> GetSceneAsync(SpatialActor actor, int propertyId);
}
public sealed class SpatialValidationException(string message) : Exception(message);
