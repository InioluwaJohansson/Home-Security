using Home_Security;
using Home_Security.Authentication;
using Home_Security.Context;
using Home_Security.Implementations.Controls;
using Home_Security.Implementations.Controls.Defaults;
using Home_Security.Implementations.Repositories;
using Home_Security.Implementations.Services;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Controls.Defaults;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(x => x.AddPolicy("Policies", c =>
{
    c.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));
// Add services to the container.
builder.Services.AddScoped<IActionRepo, ActionRepo>();
builder.Services.AddScoped<IApplianceRepo, ApplianceRepo>();
builder.Services.AddScoped<IAddressRepo, AddressRepo>();
builder.Services.AddScoped<ICameraRepo, CameraRepo>();
builder.Services.AddScoped<IContactCategoryRepo, ContactCategoryRepo>();
builder.Services.AddScoped<IContactDetailsRepo, ContactDetailsRepo>();
builder.Services.AddScoped<IContactRepo, ContactRepo>();
builder.Services.AddScoped<IDoorRepo, DoorRepo>();
builder.Services.AddScoped<IFingerPrintRepo, FingerPrintRepo>();
builder.Services.AddScoped<ILightRepo, LightRepo>();
builder.Services.AddScoped<ILogRepo, LogRepo>();
builder.Services.AddScoped<IPersonRepo, PersonRepo>();
builder.Services.AddScoped<IRoomRepo, RoomRepo>();
builder.Services.AddScoped<ISectionRepo, SectionRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IWindowRepo, WindowRepo>();
builder.Services.AddScoped<IActionService, ActionService>();
builder.Services.AddScoped<IApplianceService, ApplianceService>();
builder.Services.AddScoped<ICameraService, CameraService>();
builder.Services.AddScoped<IContactCategoryService, ContactCategoryService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IDoorService, DoorService>();
builder.Services.AddScoped<IFingerPrintService, FingerPrintService>();
builder.Services.AddScoped<ILightService, LightService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWindowService, WindowService>();
builder.Services.AddScoped<IAuthControl, AuthControl>();
builder.Services.AddScoped<IObjectDefault, ObjectDefault>();
builder.Services.AddScoped<IApplianceControl, ApplianceControl>();
builder.Services.AddScoped<ICameraControl, CameraControl>();
builder.Services.AddScoped<IContactCategoryControl, ContactCategoryControl>();
builder.Services.AddScoped<IContactControl, ContactControl>();
builder.Services.AddScoped<IDoorControl, DoorControl>();
builder.Services.AddScoped<ILightControl, LightControl>();
builder.Services.AddScoped<IPersonControl, PersonControl>();
builder.Services.AddScoped<IRoomControl, RoomControl>();
builder.Services.AddScoped<ISectionControl, SectionControl>();
builder.Services.AddScoped<IWindowControl, WindowControl>();
builder.Services.AddHostedService<BackgroundServices>();
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

var key = "Authorization Key";
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
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Policies");

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
