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
    [Authorize]
    [HttpPost("VerifyFingerPrint")]
    public async Task<IActionResult> VerifyFingerPrint(byte [] data)
    {
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
        };
        var fingerPrintService = await _fingerPrintServiceService.VerifyFingerPrint(data);
        if (fingerPrintService.Status == true)
        {
            return Ok(fingerPrintService);
        }
        return Ok(fingerPrintService);
    }
}