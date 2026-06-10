using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Models.Enums;
using Microsoft.EntityFrameworkCore;
namespace Home_Security.Implementations.Repositories;
public class HardwareRepo : BaseRepository<Hardware>, IHardwareRepo
{
    public HardwareRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Hardware> GetHardwareByFacilityTypeId(int id, FacilityType facilityType)
    {   
        switch (facilityType)
        {
            case FacilityType.Appliance:
                return await context.Hardware.FirstOrDefaultAsync(x =>x.ApplianceIds.Any(a => a == id));
            case FacilityType.Camera:
                return await context.Hardware.FirstOrDefaultAsync(x =>x.CameraIds.Any(a => a == id));
            case FacilityType.Light:
                return await context.Hardware.FirstOrDefaultAsync(x =>x.LightIds.Any(a => a == id));
            case FacilityType.Door:
                return await context.Hardware.FirstOrDefaultAsync(x =>x.DoorIds.Any(a => a == id));
            case FacilityType.Window:
                return await context.Hardware.FirstOrDefaultAsync(x =>x.WindowIds.Any(a => a == id));
            case FacilityType.External:
                return await context.Hardware.FirstOrDefaultAsync(x =>x.ExternalIds.Any(a => a == id));
            default: return null;
        }
    }
}