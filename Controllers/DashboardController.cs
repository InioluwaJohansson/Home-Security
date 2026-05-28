using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class DashboardController : Controller
{
    IActionControl _actionControl;
    IJWTAuthentication _jwtAuthentication;
    public DashboardController(IActionControl actionControl, IJWTAuthentication jwtAuthentication)
    {
        _actionControl = actionControl;
        _jwtAuthentication = jwtAuthentication;
    }
}