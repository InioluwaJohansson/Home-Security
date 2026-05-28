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
using SixLabors.ImageSharp.Memory;

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

builder.Services.AddScoped<IActionControl, ActionControl>();
builder.Services.AddScoped<IApplianceControl, ApplianceControl>();
builder.Services.AddScoped<IAuthControl, AuthControl>();
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
builder.Services.AddScoped<IRealtimeNotificationService, RealtimeNotificationService>();
builder.Services.AddScoped<IMqttService, MqttService>();
builder.Services.AddHostedService<BackgroundServices>();
builder.Services.AddHostedService<RecordingCleanupService>();
builder.Services.AddMemoryCache();
builder.Services.AddHostedService(sp => (MqttService)sp.GetRequiredService<IMqttService>());
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

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.Authority = "Owner";
    options.Audience = "Home_Security";
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/Home_Security"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
var key = "HomeSecurity1234567890";
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "Home_Security",
                ValidAudience = "All",
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(key))
            };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken =
                    context.Request.Query["access_token"];

                var path =
                    context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/chatHub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddSingleton<JWTAuthentication>(new JWTAuthentication(key));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = true,
        ValidateAudience = true,
    };
});

var app = builder.Build();
app.MapHub<HomeSecurityHub>("/homeSecurityHub");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "storage")),
    RequestPath = "/storage"
});
app.UseCors("Home_Security");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
