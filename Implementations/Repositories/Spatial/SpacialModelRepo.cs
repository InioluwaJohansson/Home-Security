
using Home_Security;
using Home_Security.Context;
using Home_Security.Interfaces.Repositories.Spatial;
using Home_Security.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Home_Security.Implementations.Repositories.Spatial;

public class SpatialModelRepository : ISpatialModelRepository
{
    private readonly HomeSecurityContext _context;

    public SpatialModelRepository(HomeSecurityContext context)
    {
        _context = context;
    }

    public Task<SpatialProperty?> GetProperty(int propertyId)
    {
        return _context.SpatialProperties
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == propertyId);
    }

    public Task<SpatialBuilding?> GetBuilding(int buildingId)
    {
        return _context.SpatialBuildings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == buildingId);
    }

    public Task<SpatialFloor?> GetFloor(int floorId)
    {
        return _context.SpatialFloors
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == floorId);
    }

    public Task<SpatialFloorSection?> GetFloorSection(int floorSectionId)
    {
        return _context.SpatialFloorSections
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == floorSectionId);
    }

    public Task<SpatialRoomBinding?> GetRoomBinding(int roomBindingId)
    {
        return _context.SpatialRoomBindings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == roomBindingId);
    }

    public Task<SpatialGeometry?> GetGeometry(int geometryId)
    {
        return _context.SpatialGeometries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == geometryId);
    }

    public Task<SpatialPlacement?> GetPlacement(int placementId)
    {
        return _context.SpatialPlacements
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == placementId);
    }

    public Task<SpatialPlacement?> GetFacilityPlacement(
        FacilityType facilityType,
        int facilityId)
    {
        return _context.SpatialPlacements
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.FacilityType == facilityType &&
                x.FacilityId == facilityId);
    }

    public Task<SpatialUtilityPlacement?> GetUtilityPlacement(int utilityId)
    {
        return _context.SpatialUtilityPlacements
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UtilityId == utilityId);
    }

    public async Task<IList<SpatialFloor>> GetPropertyFloors(int propertyId)
    {
        return await _context.SpatialFloors
            .AsNoTracking()
            .Where(x => x.PropertyId == propertyId)
            .ToListAsync();
    }

    public async Task<IList<SpatialPlacement>> GetFloorPlacements(int floorId)
    {
        return await _context.SpatialPlacements
            .AsNoTracking()
            .Where(x =>
                x.AnchorType == SpatialAnchorType.Floor &&
                x.AnchorId == floorId)
            .ToListAsync();
    }

    public async Task<IList<SpatialPlacement>> GetRoomPlacements(int roomBindingId)
    {
        return await _context.SpatialPlacements
            .AsNoTracking()
            .Where(x =>
                x.AnchorType == SpatialAnchorType.Room &&
                x.AnchorId == roomBindingId)
            .ToListAsync();
    }
}
