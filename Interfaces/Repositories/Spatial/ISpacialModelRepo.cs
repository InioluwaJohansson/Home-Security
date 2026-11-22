
using Home_Security;
using Home_Security.Models.Enums;

namespace Home_Security.Interfaces.Repositories.Spatial;

public interface ISpatialModelRepository
{
    Task<SpatialProperty?> GetProperty(int propertyId);

    Task<SpatialBuilding?> GetBuilding(int buildingId);

    Task<SpatialFloor?> GetFloor(int floorId);

    Task<SpatialFloorSection?> GetFloorSection(int floorSectionId);

    Task<SpatialRoomBinding?> GetRoomBinding(int roomBindingId);

    Task<SpatialGeometry?> GetGeometry(int geometryId);

    Task<SpatialPlacement?> GetPlacement(int placementId);

    Task<SpatialPlacement?> GetFacilityPlacement(
        FacilityType facilityType,
        int facilityId);

    Task<SpatialUtilityPlacement?> GetUtilityPlacement(int utilityId);

    Task<IList<SpatialFloor>> GetPropertyFloors(int propertyId);

    Task<IList<SpatialPlacement>> GetFloorPlacements(int floorId);

    Task<IList<SpatialPlacement>> GetRoomPlacements(int roomBindingId);
}
