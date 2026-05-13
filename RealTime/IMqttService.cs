using Home_Security.Models.DTOs;
namespace Home_Security.RealTimeServices;
public interface IMqttService
{
    Task PublishFullStateSync(string hardwareId, GenericDeviceState state);
    Task PublishDeviceUpdate(string hardwareId, GenericDeviceUpdateState update);
}