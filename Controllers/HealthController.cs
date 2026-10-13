using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/system")]
public sealed class SystemController : ControllerBase
{
    [HttpGet("status")]
    public IActionResult GetStatus() => Ok(new
    {
        application = "Home Security",
        state = "Running",
        machineName = Environment.MachineName,
        processId = Environment.ProcessId,
        utcTime = DateTimeOffset.UtcNow
    });
}