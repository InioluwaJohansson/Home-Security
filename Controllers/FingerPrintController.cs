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
public class FingerPrintController : Controller
{
    IFingerPrintService _fingerPrintServiceService;
    IFingerPrintControl _fingerPrintControl;
    IJWTAuthentication _jwtAuthentication;
    public FingerPrintController(IFingerPrintService fingerPrintServiceService, IFingerPrintControl fingerPrintControl, IJWTAuthentication jwtAuthentication)
    {
        _fingerPrintServiceService = fingerPrintServiceService;
        _fingerPrintControl = fingerPrintControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateFingerPrint")]
    public async Task<IActionResult> CreateFingerPrint(CreateFingerPrintDto createFingerPrintDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var fingerPrintService = await _fingerPrintControl.CreateFingerPrint(getAuthControlInfoDto, createFingerPrintDto);
        if (fingerPrintService.Status == true)
        {
            return Ok(fingerPrintService);
        }
        return Ok(fingerPrintService);
    }
}