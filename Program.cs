using Home_Security.Authentication;
using Home_Security.Context;
using Home_Security.Hubs;
using Home_Security.Implementations.Controls;
using Home_Security.Implementations.Repositories;
using Home_Security.Implementations.Services;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Implementations.Controls.Chats;
using Home_Security.Implementations.Repositories.Chats;
using Home_Security.Implementations.Services.Chats;
using Home_Security.Interfaces.Controls.Chats;
using Home_Security.Interfaces.Repositories.Chats;
using Home_Security.Interfaces.Services.Chats;
using Home_Security.Implementations.Controls.Calls;
using Home_Security.Implementations.Repositories.Calls;
using Home_Security.Implementations.Services.Calls;
using Home_Security.Interfaces.Controls.Calls;
using Home_Security.Interfaces.Repositories.Calls;
using Home_Security.Interfaces.Services.Calls;
using Home_Security.RealTimeServices;
using Home_Security.BackgroundServices;
using Home_Security.Streaming;
using Home_Security.Encryption;
using Home_Security.StartUp;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using System.Text;
using Home_Security.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IActionRepo, ActionRepo>();
builder.Services.AddScoped<IAddressRepo, AddressRepo>();
builder.Services.AddScoped<IApplianceRepo, ApplianceRepo>();
builder.Services.AddScoped<ICameraRepo, CameraRepo>();
builder.Services.AddScoped<IContactCategoryRepo, ContactCategoryRepo>();
builder.Services.AddScoped<IContactDetailsRepo, ContactDetailsRepo>();
builder.Services.AddScoped<IContactRepo, ContactRepo>();
builder.Services.AddScoped<IDoorRepo, DoorRepo>();
builder.Services.AddScoped<IExternalRepo, ExternalRepo>();
builder.Services.AddScoped<IFingerPrintRepo, FingerPrintRepo>();
builder.Services.AddScoped<IHardwareRepo, HardwareRepo>();
builder.Services.AddScoped<ILightRepo, LightRepo>();
builder.Services.AddScoped<ILogRepo, LogRepo>();
builder.Services.AddScoped<IPersonRepo, PersonRepo>();
builder.Services.AddScoped<IRecordingRepo, RecordingRepo>();
builder.Services.AddScoped<IRoomRepo, RoomRepo>();
builder.Services.AddScoped<ISectionRepo, SectionRepo>();
builder.Services.AddScoped<ITokenRepo, TokenRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IWindowRepo, WindowRepo>();

builder.Services.AddScoped<IActionService, ActionService>();
builder.Services.AddScoped<IApplianceService, ApplianceService>();
builder.Services.AddScoped<ICameraService, CameraService>();
builder.Services.AddScoped<IContactCategoryService, ContactCategoryService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IDoorService, DoorService>();
builder.Services.AddScoped<IExternalService, ExternalService>();
builder.Services.AddScoped<IFingerPrintService, FingerPrintService>();
builder.Services.AddScoped<IHardwareService, HardwareService>();
builder.Services.AddScoped<ILightService, LightService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IMicroControllerService, MicroControllerService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWindowService, WindowService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddScoped<IAuthControl, AuthControl>();
builder.Services.AddScoped<IActionControl, ActionControl>();
builder.Services.AddScoped<IApplianceControl, ApplianceControl>();
builder.Services.AddScoped<ICameraControl, CameraControl>();
builder.Services.AddScoped<IContactCategoryControl, ContactCategoryControl>();
builder.Services.AddScoped<IContactControl, ContactControl>();
builder.Services.AddScoped<IDoorControl, DoorControl>();
builder.Services.AddScoped<IExternalControl, ExternalControl>();
builder.Services.AddScoped<IFingerPrintControl, FingerPrintControl>();
builder.Services.AddScoped<IHardwareControl, HardwareControl>();
builder.Services.AddScoped<ILightControl, LightControl>();
builder.Services.AddScoped<ILogControl, LogControl>();
builder.Services.AddScoped<IMicroControllerControl, MicroControllerControl>();
builder.Services.AddScoped<IPersonControl, PersonControl>();
builder.Services.AddScoped<IRoomControl, RoomControl>();
builder.Services.AddScoped<ISectionControl, SectionControl>();
builder.Services.AddScoped<ITokenControl, TokenControl>();
builder.Services.AddScoped<IWindowControl, WindowControl>();
builder.Services.AddScoped<IDashboardControl, DashboardControl>();

builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatRepo, ChatRepo>();
builder.Services.AddScoped<IChatControl, ChatControl>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepo, MessageRepo>();
builder.Services.AddScoped<IMessageControl, MessageControl>();
builder.Services.AddScoped<ICallControl, CallControl>();
builder.Services.AddScoped<ICallRepo, CallRepo>();
builder.Services.AddScoped<ICallService, CallService>();

builder.Services.AddSingleton<IAuthCache, AuthCache>();

builder.Services.AddScoped<IJWTAuthentication, JWTAuthentication>();

builder.Services.AddScoped<ICameraStreamingService, CameraStreamingService>();

builder.Services.AddScoped<IRecordingCleanUpService, RecordingCleanUpService>();

builder.Services.AddScoped<IRealtimeNotificationService, RealtimeNotificationService>();
builder.Services.AddScoped<IChatHub, ChatHub>();
builder.Services.AddScoped<ICallHub, CallHub>();

builder.Services.AddSingleton<IMqttService, MqttService>();

builder.Services.AddScoped<IEncryptionService, EncryptionService>();

builder.Services.AddScoped<IStartUpService, StartUpService>();

builder.Services.AddHostedService<StartupInitializer>();
builder.Services.AddHostedService<AuthServices>();
builder.Services.AddHostedService<BackgroundServices>();
builder.Services.AddHostedService<CameraStreamingBackgroundService>();
builder.Services.AddHostedService<MqttBackgroundService>();
builder.Services.AddHostedService<HardwareServices>();
builder.Services.AddHostedService<ReccuringActionsBackgroundService>();
builder.Services.AddHostedService<CallTimeoutBackgroundService>();
builder.Services.AddHostedService<RecordingCleanUpBackgroundServiceDelete>();
builder.Services.AddHostedService<RecordingCleanUpBackgroundService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = null;
    options.Audience = jwtSection["Audience"];
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["SecretKey"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/HomeSecurityHub")) context.Token = accessToken;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new
            {
                Field = x.Key,
                Errors = x.Value.Errors.Select(e => e.ErrorMessage)
            });

        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(errors));

        return new BadRequestObjectResult(errors);
    };
});
var appData = builder.Configuration.GetSection("ApplicationDetails");
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = appData["AppName"], Version = "v1"});
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("default", config =>
    {
        config.PermitLimit = 300;
        config.Window = TimeSpan.FromMinutes(1);
        config.QueueLimit = 0;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddCors(x => x.AddPolicy(appData["AppName"], c =>
{
    c.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));

builder.Services.AddCors(options =>
{
    options.AddPolicy(appData["AppName"], policy =>
    {
        policy.WithOrigins(appData["HomeUrl"], appData["MobileApp"], appData["HomeTrackerApp"], appData["MicroControllerUrl"], "https://ais-dev-ceqn44pw3o6gczux6aezos-717948104271.europe-west2.run.app", "https://ais-dev-62beulbfxtpne7lrxwnd4x-717948104271.europe-west2.run.app").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

var connectionString = builder.Configuration.GetConnectionString("HomeSecurityContext");
builder.Services.AddDbContext<HomeSecurityContext>(c => c.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 42))));

var app = builder.Build();
try
{
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<StartupInitializer>();
    await initializer.StartAsync(CancellationToken.None);
}
catch (Exception ex)
{
    Console.WriteLine($"Startup DB init failed: {ex.Message}");
}
var storagePath = Path.Combine(Directory.GetCurrentDirectory(), "storage");
if (!Directory.Exists(storagePath)) Directory.CreateDirectory(storagePath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(storagePath),
    RequestPath = "/storage"
});

app.UseForwardedHeaders(new ForwardedHeadersOptions{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(appData["AppName"]);
app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter();
app.UseWebSockets();
app.UseMiddleware<ApiAccessMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<HomeSecurityHub>("/HomeSecurityHub");

app.Use(async (context, next) =>
{
    Console.WriteLine($"➡️ Incoming Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});
if (app.Environment.IsDevelopment())
{
}
    app.UseSwaggerUI();
    app.UseSwagger();
app.Run();