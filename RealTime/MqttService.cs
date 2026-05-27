using System.Text;
using System.Text.Json;
using Home_Security.Authentication;
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
                        var hardwareVerification = scope.ServiceProvider.GetRequiredService<IJWTAuthentication>();
                        if (!hardwareVerification.ValidateHardwareToken(update.Token) == true)
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
                        var hardwareVerification = scope.ServiceProvider.GetRequiredService<IJWTAuthentication>();
                        if (!hardwareVerification.ValidateHardwareToken(update.Token) == true)
                        {
                            var _fingerPrintService = scope.ServiceProvider.GetRequiredService<IFingerPrintService>();
                            await _fingerPrintService.ReceiveFingerPrint(update);
                            _logger.LogInformation("FINGERPRINT DATA SENT");
                        }
                        _logger.LogWarning("INVALID HARDWARE TOKEN");
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
    // GENERIC MQTT PUBLISHER
    private async Task PublishAsync(string topic, object payload, bool retain = false)
    {
        var json = JsonSerializer.Serialize(payload);
        var message = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(json).WithRetainFlag(retain).WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce).Build();
        await _client.PublishAsync(message);
        _logger.LogInformation("MQTT MESSAGE PUBLISHED | Topic: {Topic}", topic);
    }
}