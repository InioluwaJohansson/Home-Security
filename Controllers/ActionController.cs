using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class ActionController : Controller
{
    IActionControl _actionControl;
    IJWTAuthentication _jwtAuthentication;
    public ActionController(IActionControl actionControl, IJWTAuthentication jwtAuthentication)
    {
        _actionControl = actionControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateAction")]
    public async Task<IActionResult> CreateAction([FromQuery]CreateActionDto createActionDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var action = await _actionControl.CreateAction(getAuthControlInfoDto, createActionDto);
        if (action.Status == true)
        {
            return Ok(action);
        }
        return Ok(action);
    }
    [Authorize]
    [HttpPut("UpdateAction")]
    public async Task<IActionResult> UpdateAction([FromQuery] UpdateActionDto updateActionDto)
    {
       var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var action = await _actionControl.UpdateAction(getAuthControlInfoDto, updateActionDto);
        if (action.Status == true)
        {
            return Ok(action);
        }
        return Ok(action);
    }
    [Authorize]
    [HttpPost("GetActionById")]
    public async Task<IActionResult> GetActionById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var action = await _actionControl.GetActionById(getAuthControlInfoDto, id);
        if (action.Status == true)
        {
            return Ok(action);
        }
        return Ok(action);
    }
    [Authorize]
    [HttpPost("GetActionByActionName")]
    public async Task<IActionResult> GetActionByActionName(int actionId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var action = await _actionControl.ActivateDeactivateAction(getAuthControlInfoDto, actionId);
        if (action.Status == true)
        {
            return Ok(action);
        }
        return Ok(action);
    }
    [Authorize]
    [HttpPost("GetAllActions")]
    public async Task<IActionResult> GetAllActions()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var action = await _actionControl.GetAllActions(getAuthControlInfoDto);
        if (action.Status == true)
        {
            return Ok(action);
        }
        return Ok(action);
    }
    [Authorize]
    [HttpPut("DeleteAction")]
    public async Task<IActionResult> DeleteAction(int actionId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var action = await _actionControl.DeleteAction(getAuthControlInfoDto, actionId);
        if (action.Status == true)
        {
            return Ok(action);
        }
        return Ok(action);
    }
    [Authorize]
    [HttpPut("CreateActionStep")]
    public async Task<IActionResult> CreateActionStep(List<CreateActionStepDto> createActionStepDtos)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var action = await _actionControl.AddActionStep(getAuthControlInfoDto, createActionStepDtos);
        if (action.Status == true)
        {
            return Ok(action);
        }
        return Ok(action);
    }
    [Authorize]
    [HttpPut("UpdateActionStep")]
    public async Task<IActionResult> UpdateActionStep(List<UpdateActionStepDto> updateActionStepDtos)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var action = await _actionControl.UpdateActionStep(getAuthControlInfoDto, updateActionStepDtos);
        if (action.Status == true)
        {
            return Ok(action);
        }
        return Ok(action);
    }
    [Authorize]
    [HttpPut("DeleteActionStep")]
    public async Task<IActionResult> DeleteActionStep(int actionId,int actionStepId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var action = await _actionControl.DeleteActionStep(getAuthControlInfoDto, actionId, actionStepId);
        if (action.Status == true)
        {
            return Ok(action);
        }
        return Ok(action);
    }
}
