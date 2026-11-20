using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Spatial.Entities;
using Home_Security.Spatial.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Home_Security.Spatial.Controllers;

[ApiController]
[Authorize]
[Route("Home_Security/[controller]")]
public sealed class SpatialController(
    SpatialService spatial,
    IJWTAuthentication jwt,
    IAuthControl auth) : ControllerBase
{
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
    public async Task<IActionResult> Properties(CancellationToken ct)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        return Ok(await spatial.GetPropertiesAsync(actor, ct));
    }

    [HttpPost("Properties")]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyRequest request, CancellationToken ct)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest("Property name is required.");
        try { return Ok(await spatial.CreatePropertyAsync(actor, request.Name, ct)); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("Buildings")]
    public async Task<IActionResult> CreateBuilding([FromBody] CreateBuildingRequest r, CancellationToken ct)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { return Ok(await spatial.CreateBuildingAsync(actor, r.PropertyId, r.Name, r.BaseElevation, ct)); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("Floors")]
    public async Task<IActionResult> CreateFloor([FromBody] CreateFloorRequest r, CancellationToken ct)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { return Ok(await spatial.CreateFloorAsync(actor, r.BuildingId, r.Name, r.LevelIndex, r.Elevation, r.DefaultCeilingHeight, ct)); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("Floors/{floorId:int}/sections")]
    public async Task<IActionResult> BindSection(int floorId, [FromBody] BindSectionRequest r, CancellationToken ct)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { await spatial.BindSectionAsync(actor, floorId, r.SectionId, ct); return NoContent(); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("Floors/{floorId:int}/rooms")]
    public async Task<IActionResult> BindRoom(int floorId, [FromBody] BindRoomRequest r, CancellationToken ct)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { await spatial.BindRoomAsync(actor, floorId, r.RoomId, r.SectionId, ct); return NoContent(); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("Properties/{propertyId:int}/geometry")]
    public async Task<IActionResult> CreateGeometry(int propertyId, [FromBody] CreateGeometryRequest r, CancellationToken ct)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { return Ok(await spatial.CreateGeometryAsync(actor, propertyId, r.Kind, r.Name, r.GeometryJson, r.Height, r.Thickness, ct)); }
        catch (Exception ex) when (ex is SpatialValidationException or System.Text.Json.JsonException) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("Placements")]
    public async Task<IActionResult> CreatePlacement([FromBody] SpatialPlacement r, CancellationToken ct)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { return Ok(await spatial.CreatePlacementAsync(actor, r, ct)); }
        catch (SpatialValidationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("Placements/{id:int}")]
    public async Task<IActionResult> DeletePlacement(int id, CancellationToken ct)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { await spatial.DeletePlacementAsync(actor, id, ct); return NoContent(); }
        catch (SpatialValidationException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpGet("Properties/{propertyId:int}/scene")]
    public async Task<IActionResult> GetScene(int propertyId, CancellationToken ct)
    {
        var actor = await ActorAsync(); if (actor is null) return Unauthorized();
        try { return Ok(await spatial.GetSceneAsync(actor, propertyId, ct)); }
        catch (SpatialValidationException ex) { return NotFound(new { message = ex.Message }); }
    }
}

public sealed record CreatePropertyRequest(string Name);
public sealed record CreateBuildingRequest(int PropertyId, string Name, double BaseElevation);
public sealed record CreateFloorRequest(int BuildingId, string Name, int LevelIndex, double Elevation, double? DefaultCeilingHeight);
public sealed record BindSectionRequest(int SectionId);
public sealed record BindRoomRequest(int RoomId, int? SectionId);
public sealed record CreateGeometryRequest(SpatialGeometryKind Kind, string Name, string GeometryJson, double Height, double Thickness);
