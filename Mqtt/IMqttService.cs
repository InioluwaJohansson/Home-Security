using Home_Security.Models.DTOs;
public interface IMqttService
{
    Task PublishFullStateSync(
        string hardwareId,
        GenericDeviceState state);

    Task PublishDeviceUpdate(
        string hardwareId,
        GenericDeviceUpdateState update);

    Task PublishDeviceStatus(
        string hardwareId,
        bool isOnline);
}