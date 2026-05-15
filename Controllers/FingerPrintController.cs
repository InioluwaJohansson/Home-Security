using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class FingerPrintController : Controller
{
    IFingerPrintService _fingerPrintServiceService;
    public FingerPrintController(IFingerPrintService fingerPrintServiceService)
    {
        _fingerPrintServiceService = fingerPrintServiceService;
    }
    [HttpGet("VerifyFingerPrint")]
    public async Task<IActionResult> VerifyFingerPrint(byte [] data)
    {
        var fingerPrintService = await _fingerPrintServiceService.VerifyFingerPrint(data);
        if (fingerPrintService.Status == true)
        {
            return Ok(fingerPrintService);
        }
        return Ok(fingerPrintService);
    }
    [HttpPost("CreateFingerPrint")]
    public async Task<IActionResult> CreateFingerPrint(CreateFingerPrintDto createFingerPrintDto)
    {
        var fingerPrintService = await _fingerPrintServiceService.CreateFingerPrint(createFingerPrintDto);
        if (fingerPrintService.Status == true)
        {
            return Ok(fingerPrintService);
        }
        return Ok(fingerPrintService);
    }
}