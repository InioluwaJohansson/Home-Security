using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls.Defaults;
using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;

public interface IHardwareControl
{
    public Task<BaseResponse> UpdateHardwareServices(UpdateHardwareDto hardwareDto);
    public Task<HardwareResponse> GetHardwareServices(UpdateHardwareResponseIds updateHardwareResponseIds);
}