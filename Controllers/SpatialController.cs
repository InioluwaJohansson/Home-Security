using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Home_Security.Spatial.Controllers;

[ApiController]
[Authorize]
[Route("Home_Security/[controller]")]
public sealed class SpatialController : ControllerBase
{
    private readonly ISpatialService spatial;
    private readonly IJWTAuthentication jwt;
    private readonly IAuthControl auth;
    public SpatialController(ISpatialService _spatial, IJWTAuthentication _jwt, IAuthControl _auth)
    {
        spatial = _spatial;
        jwt = _jwt;
        auth = _auth;
    }
    private async Task<SpatialActor?> ActorAsync()
    {
        var header = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(header) || !header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) return null;
        var user = jwt.GetUserFromToken(header["Bearer ".Length..].Trim());
        if (user is null) return null;
        var current = await auth.GetAuthDetails(user.PersonId, user.AuthorizationCode);
        if (current.Status != true) return null;
        return new SpatialActor(user.PersonId, user.Id, user.RoleName);
    }

    [HttpGet("Properties")]
    public async Task<IActionResult> Properties()
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        return Ok(await spatial.GetPropertiesAsync(actor));
    }

    [HttpPost("CreateProperty")]
    public async Task<IActionResult> CreateProperty(string propertyName)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        if (string.IsNullOrWhiteSpace(propertyName)) return BadRequest("Property name is required.");
        try { return Ok(await spatial.CreatePropertyAsync(actor, propertyName)); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("CreateBuilding")]
    public async Task<IActionResult> CreateBuilding(CreateBuildingRequest r)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { return Ok(await spatial.CreateBuildingAsync(actor, r.PropertyId, r.Name, r.BaseElevation)); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("CreateFloor")]
    public async Task<IActionResult> CreateFloor(CreateFloorRequest r)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { return Ok(await spatial.CreateFloorAsync(actor, r.BuildingId, r.Name, r.LevelIndex, r.Elevation, r.DefaultCeilingHeight)); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("BindSection")]
    public async Task<IActionResult> BindSection(int floorId, int sectionId)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { await spatial.BindSectionAsync(actor, floorId, sectionId); return NoContent(); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("BindRoom")]
    public async Task<IActionResult> BindRoom(int floorId, int roomId, int? sectionId)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { await spatial.BindRoomAsync(actor, floorId, roomId, sectionId ?? 0); return NoContent(); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("CreateGeometry")]
    public async Task<IActionResult> CreateGeometry([FromQuery]int propertyId, CreateGeometryRequest r)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { return Ok(await spatial.CreateGeometryAsync(actor, propertyId, r.Kind, r.Name, r.GeometryJson, r.Height, r.Thickness)); }
        catch (Exception ex) when (ex is SpatialValidationException or System.Text.Json.JsonException) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("Placements")]
    public async Task<IActionResult> CreatePlacement(SpatialPlacement r)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { return Ok(await spatial.CreatePlacementAsync(actor, r)); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("DeletePlacement")]
    public async Task<IActionResult> DeletePlacement(int id)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { await spatial.DeletePlacementAsync(actor, id); return NoContent(); }
        catch (SpatialValidationException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpGet("GetScene")]
    public async Task<IActionResult> GetScene(int propertyId)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { return Ok(await spatial.GetSceneAsync(actor, propertyId)); }
        catch (SpatialValidationException ex) { return NotFound(new { message = ex.Message }); }
    }
}

public sealed record CreateBuildingRequest(int PropertyId, string Name, double BaseElevation);
public sealed record CreateFloorRequest(int BuildingId, string Name, int LevelIndex, double Elevation, double? DefaultCeilingHeight);
public sealed record CreateGeometryRequest(SpatialGeometryKind Kind, string Name, string GeometryJson, double Height, double Thickness);
