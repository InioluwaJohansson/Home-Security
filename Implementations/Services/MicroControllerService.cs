using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class MicroControllerService : IMicroControllerService
{
    IHardwareRepo _hardwareRepo;
    IApplianceRepo _applianceRepo;
    ILightRepo _lightRepo;
    ICameraRepo _cameraRepo;
    IDoorRepo _doorRepo;
    IWindowRepo _windowRepo;
    ILogRepo _logRepo;
    public MicroControllerService(IHardwareRepo hardwareRepo, IApplianceRepo applianceRepo, ILightRepo lightRepo, ICameraRepo cameraRepo, IDoorRepo doorRepo, IWindowRepo windowRepo, ILogRepo logRepo)
    {
        _hardwareRepo = hardwareRepo;
    _applianceRepo = applianceRepo;
    _lightRepo = lightRepo;
    _cameraRepo = cameraRepo;
    _doorRepo = doorRepo;
    _windowRepo = windowRepo;
    _logRepo = logRepo;
    }
    public async Task<HardwareIdsResponseModel> GetById(string hardWareId, string AuthKey)
    {
        var hardware = await _hardwareRepo.Get(x => x.HardwareId == hardWareId && x.Auth == AuthKey && x.IsDeleted == false);
        if(hardware != null)
        {
            return new HardwareIdsResponseModel()
            {
                Data = GetIdsDetails(hardware),
                Status = true,
                Message = "Hardware Retrieved Successfully!"
            };
        }
        return new HardwareIdsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Hardware!"
        };
    }
}