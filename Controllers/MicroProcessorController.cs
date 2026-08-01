using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class MicroControllerController : Controller
{
    IMicroControllerService _microControllerService;
    
    public MicroControllerController(IMicroControllerService microControllerService)
    {
        _microControllerService = microControllerService;
    }
    [HttpGet("GetMicroControllerMTTQConfig")]
    public async Task<IActionResult> GetMicroControllerDetails(string hardWareId, string AuthKey)
    {
        var microController = await _microControllerService.GetHardwareMTQQAuth(hardWareId, AuthKey);
        if (microController.Status == true)
        {
            return Ok(microController);
        }
        return Ok(microController);
    }
}
