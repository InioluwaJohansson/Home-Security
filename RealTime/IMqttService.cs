using Home_Security.Models.DTOs;
namespace Home_Security.RealTimeServices;
public interface IMqttService
{
    Task ConnectAsync(CancellationToken cancellationToken);
    Task PublishFullStateSync(string hardwareId, GenericDeviceState state);
    Task PublishDeviceUpdate(string hardwareId, GenericDeviceUpdateState update);
    Task PublishUtilityUpdate(string hardwareId, GenericUtilityUpdateState update);
    public Task PublishDeviceStatus(string hardwareId, object data);
    Task PublishFingerPrint(string hardwareId, object objectToPublish);
    public Task PublishNFIDCode(string hardwareId, string nfidCode);
}