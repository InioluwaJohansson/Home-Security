namespace Home_Security.Middleware;
public class ApiAccessMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly string _externalHost = configuration["ApplicationDetails:MobileApp"] ?? string.Empty;

    private readonly string[] _externalAllowedPaths = configuration.GetSection("ApiAccess:ExternalAllowedPaths").Get<string[]>() ?? [];

    public async Task InvokeAsync(HttpContext context)
    {
        var isExternal = context.Request.Host.Host.Equals(_externalHost, StringComparison.OrdinalIgnoreCase);
        var isAllowed = _externalAllowedPaths.Any(path => context.Request.Path.StartsWithSegments(path));
        if (isExternal && !isAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "This endpoint does not exist."
            });
            return;
        }
        await next(context);
    }
}