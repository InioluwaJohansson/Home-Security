using Home_Security.Authentication;
using Home_Security.Context;
using Home_Security.Hubs;
using Home_Security.Implementations.Controls;
using Home_Security.Implementations.Repositories;
using Home_Security.Implementations.Services;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Home_Security.RealTimeServices;
using Home_Security.BackgroundServices;
using Home_Security.Streaming;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(x => x.AddPolicy("Policies", c =>
{
    c.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));

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
builder.Services.AddScoped<IHardwareControl, HardwareControl>();
builder.Services.AddScoped<ILightControl, LightControl>();
builder.Services.AddScoped<ILogControl, LogControl>();
builder.Services.AddScoped<IPersonControl, PersonControl>();
builder.Services.AddScoped<IRoomControl, RoomControl>();
builder.Services.AddScoped<ISectionControl, SectionControl>();
builder.Services.AddScoped<ITokenControl, TokenControl>();
builder.Services.AddScoped<IWindowControl, WindowControl>();
builder.Services.AddScoped<IDashboardControl, DashboardControl>();

builder.Services.AddScoped<IJWTAuthentication, JWTAuthentication>();

builder.Services.AddScoped<ICameraStreamingService, CameraStreamingService>();
builder.Services.AddScoped<IRealtimeNotificationService, RealtimeNotificationService>();
builder.Services.AddScoped<IMqttService, MqttService>();
builder.Services.AddHostedService<BackgroundServices>();
builder.Services.AddHostedService<RecordingCleanupService>();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
var connectionString = builder.Configuration.GetConnectionString("HomeSecurityContext");
builder.Services.AddDbContext<HomeSecurityContext>(c => c.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {Title = "Home Security", Version = "v1"});
});

var jwtSection = builder.Configuration.GetSection("Jwt");
var key = jwtSection["SecretKey"];
var issuer = jwtSection["Issuer"];
var audience = jwtSection["Audience"];
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = issuer;
        options.Audience = audience;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/chatHub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
         options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/HomeSecurityHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });


var app = builder.Build();
app.MapHub<HomeSecurityHub>("/homeSecurityHub");
app.MapHub<ChatHub>("/homeSecurityChatHub");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var storagePath = Path.Combine(Directory.GetCurrentDirectory(), "storage");

if (!Directory.Exists(storagePath))
{
    Directory.CreateDirectory(storagePath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(storagePath),
    RequestPath = "/storage"
});
app.UseCors("Home_Security");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
