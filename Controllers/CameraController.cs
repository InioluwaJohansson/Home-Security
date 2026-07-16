using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

[Route("Home_Security/[controller]")]
[ApiController]
public class CameraController : Controller
{
    ICameraControl _cameraControl;
    IJWTAuthentication _jwtAuthentication;
    public CameraController(ICameraControl cameraControl, IJWTAuthentication jwtAuthentication)
    {
        _cameraControl = cameraControl;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateCamera")]
    public async Task<IActionResult> CreateCamera([FromBody] CreateCameraDto createCameraDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var camera = await _cameraControl.CreateCamera(getAuthControlInfoDto, createCameraDto);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
    [Authorize]
    [HttpPut("UpdateCamera")]
    public async Task<IActionResult> UpdateCamera([FromBody] UpdateCameraDto updateCameraDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var camera = await _cameraControl.UpdateCamera(getAuthControlInfoDto, updateCameraDto);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
    [Authorize]
    [HttpPut("RestartCamera")]
    public async Task<IActionResult> RestartCamera(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var camera = await _cameraControl.RestartCamera(getAuthControlInfoDto, id);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
    [Authorize]
    [HttpPost("ToggleCamera")]
    public async Task<IActionResult> ToggleCamera(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var camera = await _cameraControl.ToggleCamera(getAuthControlInfoDto, id);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
    [Authorize]
    [HttpPost("GetCameraById")]
    public async Task<IActionResult> GetCameraById(int id)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var camera = await _cameraControl.GetCameraById(getAuthControlInfoDto, id);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
    [Authorize]
    [HttpPost("GetAllCamerasBySectionId")]
    public async Task<IActionResult> GetAllCamerasBySectionId(int sectionId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var camera = await _cameraControl.GetAllCamerasBySectionId(getAuthControlInfoDto, sectionId);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
    [Authorize]
    [HttpPost("GetAllCamerasByRoomId")]
    public async Task<IActionResult> GetAllCamerasByRoomId(int roomId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var camera = await _cameraControl.GetAllCamerasByRoomId(getAuthControlInfoDto, roomId);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
    [Authorize]
    [HttpPost("GetAllCameras")]
    public async Task<IActionResult> GetAllCameras()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var camera = await _cameraControl.GetAllCameras(getAuthControlInfoDto);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
    [Authorize]
    [HttpPut("DeleteCamera")]
    public async Task<IActionResult> DeleteCamera(int cameraId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = user.PersonId,
            AuthorizationCode = user.AuthorizationCode,
        };
        var camera = await _cameraControl.DeleteCamera(getAuthControlInfoDto, cameraId);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
}
