using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class FingerPrintController : Controller
{
    IFingerPrintService _fingerPrintServiceService;
    public FingerPrintController(IFingerPrintService fingerPrintServiceService)
    {
        _fingerPrintServiceService = fingerPrintServiceService;
    }
    [HttpPost("VerifyFingerPrint")]
    public async Task<IActionResult> VerifyFingerPrint(BinaryData data)
    {
        var fingerPrintService = await _fingerPrintServiceService.VerifyFingerPrint(data);
        if (fingerPrintService.Status == true)
        {
            return Ok(fingerPrintService);
        }
        return Ok(fingerPrintService);
    }
}