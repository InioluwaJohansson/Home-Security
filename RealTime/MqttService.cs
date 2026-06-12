using System.Text;
using System.Text.Json;
using Home_Security.Authentication;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
namespace Home_Security.RealTimeServices;

public class MqttService : BackgroundService, IMqttService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MqttService> _logger;
    private IMqttClient _client = default!;

    public MqttService(IConfiguration configuration, IServiceScopeFactory scopeFactory, ILogger<MqttService> logger)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    // START MQTT SERVICE
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new MqttFactory();
        _client = factory.CreateMqttClient();
        await ConfigureMqttEvents();
        await ConnectAsync(stoppingToken);
        while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
    }
    private async Task ConfigureMqttEvents()
    {
        // CONNECTED
        _client.ConnectedAsync += async e =>
        {
            _logger.LogInformation("MQTT CONNECTED");
            await SubscribeTopics();
        };
        // DISCONNECTED
        _client.DisconnectedAsync += async e =>
        {
            _logger.LogWarning("MQTT DISCONNECTED");
            await ReconnectAsync();
        };
        // MESSAGE RECEIVED
        _client.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                _logger.LogInformation("MQTT MESSAGE RECEIVED | Topic: {Topic}",topic);
                // DEVICE STATE UPDATE
                if (topic.EndsWith("/state"))
                {
                    var update = JsonSerializer.Deserialize<GenericDeviceUpdateState>(payload,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (update != null)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        if (ValidateHardwareToken(update.Token) == true)
                        {
                            var _microControllerService = scope.ServiceProvider.GetRequiredService<IMicroControllerService>();
                            await _microControllerService.UpdateDeviceStates(update);
                            _logger.LogInformation("DEVICE STATE UPDATED");
                        }
                        _logger.LogWarning("INVALID HARDWARE TOKEN");
                        return;
                    }
                }
                if (topic.EndsWith("/fingerprint"))
                {
                    var update = JsonSerializer.Deserialize<FingerPrintDataDto>(payload,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (update != null)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        if (ValidateHardwareToken(update.Token) == true)
                        {
                            var _fingerPrintService = scope.ServiceProvider.GetRequiredService<IFingerPrintService>();
                            await _fingerPrintService.ReceiveFingerPrint(update);
                            _logger.LogInformation("FINGERPRINT DATA SENT");
                        }
                        _logger.LogWarning("INVALID HARDWARE TOKEN");
                        return;
                    }
                }
                if (topic.EndsWith("/fingerprint/verify"))
                {
                    var update = JsonSerializer.Deserialize<FingerPrintVerifyDataDto>(payload,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (update != null)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        if (!ValidateHardwareToken(update.Token) == true)
                        {
                            var _fingerPrintService = scope.ServiceProvider.GetRequiredService<IFingerPrintService>();
                            var data = await _fingerPrintService.VerifyFingerPrint(update.Data);
                            if(data.Status == true)
                            {
                                await PublishFingerPrintVerify(update.HardwareId, data.Status);
                                _logger.LogInformation("FINGERPRINT VERIFY RESPONSE SENT | Status: {Status}", data.Status);
                            }
                            else
                            {
                                await PublishFingerPrintVerify(update.HardwareId, false);
                                _logger.LogInformation("FINGERPRINT VERIFY RESPONSE SENT | Status: False");
                            }
                            _logger.LogInformation("FINGERPRINT DATA SENT");
                        }
                        _logger.LogWarning("INVALID HARDWARE TOKEN");
                        return;
                    }
                }
                if (topic.EndsWith("/heartbeat"))
                {
                    var update = JsonSerializer.Deserialize<HardwareHeartBeatDto>(payload,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (update != null)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        if (!ValidateHardwareToken(update.Token) == true)
                        {
                            var _hardwareRepo = scope.ServiceProvider.GetRequiredService<IHardwareRepo>();
                            var hardware = await _hardwareRepo.Get(h => h.HardwareId == update.HardwareId);
                            if(hardware != null)
                            {
                                hardware.PowerActive = update.PowerActive;
                                hardware.LastModifiedOn = DateTime.UtcNow;
                                _logger.LogInformation("HARDWARE HEARTBEAT RECEIVED | HardwareId: {HardwareId} | PowerActive: {PowerActive}", update.HardwareId, update.PowerActive);
                                await _hardwareRepo.Update(hardware);
                            }
                            _logger.LogInformation("HARDWARE HEARTBEAT UPDATED");
                        }
                        _logger.LogWarning("INVALID HARDWARE TOKEN");
                        return;
                    }
                }
                if (topic.EndsWith("/NFID/verify"))
                {
                    var update = JsonSerializer.Deserialize<NFIDLoginDto>(payload,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (update != null)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        if (!ValidateHardwareToken(update.Token) == true)
                        {
                            var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                            var response = await _userService.NFIDLogin(update.Token,update.NFIDCode);
                            if(response.Status == true)
                            {
                                await PublishNFIDVerify(update.Token, response);
                                _logger.LogInformation("NFID VERIFIED");
                            }
                        }
                        _logger.LogWarning("INVALID HARDWARE TOKEN");
                        return;
                    }
                }
                if (topic.EndsWith("/Hardware/Auth"))
                {
                    var update = JsonSerializer.Deserialize<MicroControllerLoginDto>(payload,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (update != null)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var _hardwareService = scope.ServiceProvider.GetRequiredService<IMicroControllerService>();
                        var response = await _hardwareService.GetByAuth(update.HardwareId, update.AuthKey);
                        await PublishMicroControllerAuth(update.HardwareId, response);
                        _logger.LogInformation("MICROCONTROLLER VERIFIED");

                        return;
                    }
                }
                // DEVICE FULL SYNC
                if (topic.EndsWith("/sync"))
                {
                    var state = JsonSerializer.Deserialize<GenericDeviceState>(payload,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }); 
                    if (state != null) _logger.LogInformation("FULL DEVICE SYNC RECEIVED");
                }
                // DEVICE STATUS
                if (topic.EndsWith("/status")) _logger.LogInformation("DEVICE STATUS RECEIVED");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT MESSAGE PROCESSING FAILED");
            }
        };
        await Task.CompletedTask;
    }
    // CONNECT TO MQTT BROKER
    private async Task ConnectAsync(CancellationToken cancellationToken)
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(
                _configuration["Mqtt:Host"],
                int.Parse(_configuration["Mqtt:Port"]!))
            .WithCredentials(
                _configuration["Mqtt:Username"],
                _configuration["Mqtt:Password"])
            .WithCleanSession(false)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
            .Build();
        await _client.ConnectAsync(options, cancellationToken);
    }
    // AUTO RECONNECT
    private async Task ReconnectAsync()
    {
        while (!_client.IsConnected)
        {
            try
            {
                _logger.LogInformation("RECONNECTING MQTT...");
                await Task.Delay(5000);
                await ConnectAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT RECONNECT FAILED");
            }
        }
    }    
    private async Task SubscribeTopics()
    {
        var options = new MqttClientSubscribeOptionsBuilder().WithTopicFilter(f =>
        {
            f.WithTopic("home/+/state");
            f.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce);
        }).WithTopicFilter(f =>
        {
            f.WithTopic("home/+/sync");
            f.WithQualityOfServiceLevel(
            MqttQualityOfServiceLevel.AtLeastOnce);
        }).WithTopicFilter(f =>
        {
            f.WithTopic("home/+/Hardware/Auth");
            f.WithQualityOfServiceLevel(
            MqttQualityOfServiceLevel.AtLeastOnce);
        }).WithTopicFilter(f =>
        {
            f.WithTopic("home/+/fingerprint");
            f.WithQualityOfServiceLevel(
            MqttQualityOfServiceLevel.AtLeastOnce);
        }).WithTopicFilter(f =>
        {
            f.WithTopic("home/+/fingerprint/verify");
            f.WithQualityOfServiceLevel(
            MqttQualityOfServiceLevel.AtLeastOnce);
        }).WithTopicFilter(f =>
        {
            f.WithTopic("home/+/NFID/verify");
            f.WithQualityOfServiceLevel(
            MqttQualityOfServiceLevel.AtLeastOnce);
        }).WithTopicFilter(f =>
        {
            f.WithTopic("home/+/heartbeat");
            f.WithQualityOfServiceLevel(
            MqttQualityOfServiceLevel.AtLeastOnce);
        }).WithTopicFilter(f =>
        {
            f.WithTopic("home/+/status");
            f.WithQualityOfServiceLevel(
            MqttQualityOfServiceLevel.AtLeastOnce);
        }).Build();
        await _client.SubscribeAsync(options);
        _logger.LogInformation("MQTT TOPICS SUBSCRIBED");
    }
    public async Task PublishFullStateSync(string hardwareId, GenericDeviceState state)
    {
        var topic = $"home/{hardwareId}/sync";
        await PublishAsync(topic, state, true);
        _logger.LogInformation("FULL STATE SYNC SENT");
    }
    private async Task PublishFingerPrintVerify(string hardwareId, bool response)
    {
        var topic = $"home/{hardwareId}/fingerprint/verify/response";
        await PublishAsync(topic, response, true);
        _logger.LogInformation("FINGERPRINT VERIFY RESPONSE SENT");
    }
    private async Task PublishNFIDVerify(string hardwareId, UserLoginResponse data)
    {
        var topic = $"home/{hardwareId}/NFID/Data";
        await PublishAsync(topic, data, true);
        _logger.LogInformation("NFID VERIFY RESPONSE SENT");
    }
    // SEND SINGLE DEVICE UPDATE
    public async Task PublishDeviceUpdate(string hardwareId, GenericDeviceUpdateState update)
    {
        var topic = $"home/{hardwareId}/command";
        await PublishAsync(topic, update);
        _logger.LogInformation("DEVICE UPDATE SENT");
    }
    // DEVICE ONLINE/OFFLINE STATUS
    public async Task PublishDeviceStatus(string hardwareId, object data)
    {
        var topic = $"home/{hardwareId}/status";
        await PublishAsync(topic, data, true);
        _logger.LogInformation("DEVICE STATUS SENT");
    }
    public async Task PublishFingerPrint(string hardwareId, object objectToPublish)
    {
        var topic = $"home/{hardwareId}/fingerprint";
        await PublishAsync(topic, objectToPublish, true);
        _logger.LogInformation("FINGERPRINT DATA SENT");
    }
    public async Task PublishMicroControllerAuth(string hardwareId, object objectToPublish)
    {
        var topic = $"home/{hardwareId}/fingerprint";
        await PublishAsync(topic, objectToPublish, true);
        _logger.LogInformation("FINGERPRINT DATA SENT");
    }
    public async Task PublishNFIDCode(string hardwareId, string nfidCode)
    {
        var topic = $"home/{hardwareId}/NFID";
        await PublishAsync(topic, new { NFIDCode = nfidCode }, true);
        _logger.LogInformation("NFID CODE SENT");
    }
    // GENERIC MQTT PUBLISHER
    private async Task PublishAsync(string topic, object payload, bool retain = false)
    {
        var json = JsonSerializer.Serialize(payload);
        var message = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(json).WithRetainFlag(retain).WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce).Build();
        await _client.PublishAsync(message);
        _logger.LogInformation("MQTT MESSAGE PUBLISHED | Topic: {Topic}", topic);
    }
    private bool ValidateHardwareToken(string token)
    {
        using var scope = _scopeFactory.CreateScope();
        var hardwareVerification = scope.ServiceProvider.GetRequiredService<IJWTAuthentication>();
        return hardwareVerification.ValidateHardwareToken(token);
    }
}