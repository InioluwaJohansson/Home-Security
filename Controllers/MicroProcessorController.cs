using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;


[Route("Home_Security/[controller]")]
[ApiController]
public class MicroControllerController : Controller
{
    IMicroControllerService _microControllerService;
    
    public MicroControllerController(IMicroControllerService microControllerService)
    {
        _microControllerService = microControllerService;
    }
    [HttpGet("GetMicroControllerMQTTConfig")]
    public async Task<IActionResult> GetMicroControllerDetails(string hardWareId, string AuthKey)
    {
        var microController = await _microControllerService.GetHardwareMTQQAuth(hardWareId, AuthKey);
        Console.WriteLine(JsonSerializer.Serialize(microController, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
        if (microController.Status == true)
        {
            return Ok(microController);
        }
        return Ok(microController);
    }
}
