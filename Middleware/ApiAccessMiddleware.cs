namespace Home_Security.Middleware;
public class ApiAccessMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly string _externalHost = configuration["ApplicationDetails:MobileApp"] ?? string.Empty;
    private readonly string _homeTrackerHost = configuration["ApplicationDetails:HomeTracker"] ?? string.Empty;
    private readonly string _homeTrackerAppHost = configuration["ApplicationDetails:HomeTrackerApp"] ?? string.Empty;
    private readonly string _microControllerHost = configuration["ApplicationDetails:MicroControllerUrl"] ?? string.Empty;
    private readonly string[] _externalAllowedPaths = configuration.GetSection("ApiAccess:ExternalAllowedPaths").Get<string[]>() ?? [];
    private readonly string[] _microControllerAllowedPaths = configuration.GetSection("ApiAccess:MicroControllerAllowedPaths").Get<string[]>() ?? [];
    private readonly string[] _homeTrackerAllowedPaths = configuration.GetSection("ApiAccess:HomeTrackerAllowedPaths").Get<string[]>() ?? [];
    private readonly string[] _homeTrackerAppAllowedPaths = configuration.GetSection("ApiAccess:HomeTrackerAppAllowedPaths").Get<string[]>() ?? [];
    public async Task InvokeAsync(HttpContext context)
    {
        var isExternal = context.Request.Host.Host.Equals(_externalHost, StringComparison.OrdinalIgnoreCase);
        var isAllowed = _externalAllowedPaths.Any(path => context.Request.Path.StartsWithSegments(path));
        if (isExternal && !isAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "App huh?"
            });
            return;
        }
        var isMicroController = context.Request.Host.Host.Equals(_microControllerHost, StringComparison.OrdinalIgnoreCase);
        var isMicroAllowed = _microControllerAllowedPaths.Any(path => context.Request.Path.StartsWithSegments(path));
        if (isMicroController && !isMicroAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "Huh?"
            });
            return;
        }
        var isHomeTracker = context.Request.Host.Host.Equals(_homeTrackerHost, StringComparison.OrdinalIgnoreCase);
        var isHomeTrackerAllowed = _homeTrackerAllowedPaths.Any(path => context.Request.Path.StartsWithSegments(path));
        if (isHomeTracker && !isHomeTrackerAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "Huh?"
            });
            return;
        }
        var isHomeTrackerApp = context.Request.Host.Host.Equals(_homeTrackerAppHost, StringComparison.OrdinalIgnoreCase);
        var isHomeTrackerAppAllowed = _homeTrackerAppAllowedPaths.Any(path => context.Request.Path.StartsWithSegments(path));
        if (isHomeTrackerApp && !isHomeTrackerAppAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "Huh?"
            });
            return;
        }
        await next(context);
    }
}