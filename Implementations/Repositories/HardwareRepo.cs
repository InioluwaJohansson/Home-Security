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
        var Hardware =await context.Hardware.Where(h => !h.IsDeleted).ToListAsync();
        switch (facilityType)
        {
            case FacilityType.Appliance:
                return Hardware.FirstOrDefault(x =>x.ApplianceIds.Any(a => a == id));
            case FacilityType.Camera:
                return Hardware.FirstOrDefault(x =>x.CameraIds.Any(a => a == id));
            case FacilityType.Light:
                return Hardware.FirstOrDefault(x =>x.LightIds.Any(a => a == id));
            case FacilityType.Door:
                return Hardware.FirstOrDefault(x =>x.DoorIds.Any(a => a == id));
            case FacilityType.Window:
                return Hardware.FirstOrDefault(x =>x.WindowIds.Any(a => a == id));
            case FacilityType.External:
                return Hardware.FirstOrDefault(x =>x.ExternalIds.Any(a => a == id));
            default: return null;
        }
    }
}