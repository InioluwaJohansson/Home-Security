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
    public CameraController(ICameraControl cameraControl)
    {
        _cameraControl = cameraControl;
    }
    [Authorize]
    [HttpPost("CreateCamera")]
    public async Task<IActionResult> CreateCamera([FromBody] CreateCameraDto createCameraDto)
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
        };
        var camera = await _cameraControl.UpdateCamera(getAuthControlInfoDto, updateCameraDto);
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
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
        var UserIdString = User.FindFirst(ClaimTypes.Name)?.Value;
        var PersonIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AuthorizationCode = User.FindFirst(ClaimTypes.Authentication)?.Value;
        if (!int.TryParse(UserIdString, out int loggedInUserId) || !int.TryParse(PersonIdString, out int loggedInPersonId))
            return Unauthorized();
        var getAuthControlInfoDto = new GetAuthControlInfoDto(){
            PersonId = loggedInPersonId,
            AuthorizationCode = AuthorizationCode.ToString(),
        };
        var camera = await _cameraControl.DeleteCamera(getAuthControlInfoDto, cameraId);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
}
