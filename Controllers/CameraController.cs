using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("Home_Security/[controller]")]
[ApiController]
public class CameraController : Controller
{
    ICameraControl _cameraControl;
    public CameraController(ICameraControl cameraControl)
    {
        _cameraControl = cameraControl;
    }
    [HttpPost("CreateCamera")]
    public async Task<IActionResult> CreateCamera([FromQuery] GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] CreateCameraDto createCameraDto)
    {
        var camera = await _cameraControl.CreateCamera(getAuthControlInfoDto, createCameraDto);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }

    [HttpPut("UpdateCamera")]
    public async Task<IActionResult> UpdateCamera([FromQuery]GetAuthControlInfoDto getAuthControlInfoDto, [FromBody] UpdateCameraDto updateCameraDto)
    {
        var camera = await _cameraControl.UpdateCamera(getAuthControlInfoDto, updateCameraDto);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }

    [HttpGet("GetCameraById")]
    public async Task<IActionResult> GetCameraById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var camera = await _cameraControl.GetCameraById(getAuthControlInfoDto, id);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }

    [HttpGet("GetAllCamerasBySectionId")]
    public async Task<IActionResult> GetAllCamerasBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var camera = await _cameraControl.GetAllCamerasBySectionId(getAuthControlInfoDto, sectionId);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
    [HttpGet("GetAllCamerasByRoomId")]
    public async Task<IActionResult> GetAllCamerasByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var camera = await _cameraControl.GetAllCamerasByRoomId(getAuthControlInfoDto, roomId);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }

    [HttpGet("GetAllCameras")]
    public async Task<IActionResult> GetAllCameras(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var camera = await _cameraControl.GetAllCameras(getAuthControlInfoDto);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }

    [HttpPut("DeleteCamera")]
    public async Task<IActionResult> DeleteCamera(GetAuthControlInfoDto getAuthControlInfoDto, int cameraId)
    {
        var camera = await _cameraControl.DeleteCamera(getAuthControlInfoDto, cameraId);
        if (camera.Status == true)
        {
            return Ok(camera);
        }
        return Ok(camera);
    }
}
