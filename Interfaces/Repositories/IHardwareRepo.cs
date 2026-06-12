using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Models.Enums;
namespace Home_Security.Interfaces.Repositories;
public interface IHardwareRepo : IRepo<Hardware>
{
    Task<Hardware> GetHardwareByFacilityTypeId(int id, FacilityType facilityType);
}