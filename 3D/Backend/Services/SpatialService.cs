using System.Text.Json;
using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.RealTimeServices;
using Home_Security.Spatial.Entities;
using Microsoft.EntityFrameworkCore;

namespace Home_Security.Spatial.Services;

public sealed class SpatialService(
    HomeSecurityContext db,
    ISpatialTargetValidator targetValidator,
    ISpatialGeometryValidator geometryValidator,
    IRealtimeNotificationService realtime)
{
    public async Task<SpatialProperty> CreatePropertyAsync(SpatialActor actor, string name, CancellationToken ct)
    {
        var p = new SpatialProperty { PersonId = actor.PersonId, CreatedBy = actor.PersonId, Name = name.Trim() };
        db.SpatialProperties.Add(p);
        await db.SaveChangesAsync(ct);
        await Emit(actor.PersonId, p.Id, "Property", p.Version, "Created");
        return p;
    }

    public Task<List<SpatialProperty>> GetPropertiesAsync(SpatialActor actor, CancellationToken ct) =>
        db.SpatialProperties.AsNoTracking().Where(x => x.PersonId == actor.PersonId && !x.IsDeleted)
            .OrderBy(x => x.Id).ToListAsync(ct);

    public async Task<SpatialBuilding> CreateBuildingAsync(SpatialActor actor, int propertyId, string name, double baseElevation, CancellationToken ct)
    {
        var property = await OwnedProperty(actor.PersonId, propertyId, ct);
        var b = new SpatialBuilding { PersonId = actor.PersonId, CreatedBy = actor.PersonId, PropertyId = property.Id, Name = name.Trim(), BaseElevation = baseElevation };
        db.SpatialBuildings.Add(b);
        property.Version++;
        await db.SaveChangesAsync(ct);
        await Emit(actor.PersonId, propertyId, "Building", b.Version, "Created");
        return b;
    }
    public async Task<SpatialFloor> CreateFloorAsync(SpatialActor actor, int buildingId, string name, int levelIndex, double elevation, double? ceilingHeight, CancellationToken ct)
    {
        var building = await db.SpatialBuildings.FirstOrDefaultAsync(x => x.Id == buildingId && x.PersonId == actor.PersonId && !x.IsDeleted, ct)
            ?? throw new SpatialValidationException("Building not found.");
        var floor = new SpatialFloor { PersonId = actor.PersonId, CreatedBy = actor.PersonId, BuildingId = buildingId, Name = name.Trim(), LevelIndex = levelIndex, Elevation = elevation, DefaultCeilingHeight = ceilingHeight };
        db.SpatialFloors.Add(floor);
        await db.SaveChangesAsync(ct);
        await Emit(actor.PersonId, building.PropertyId, "Floor", floor.Version, "Created");
        return floor;
    }

    public async Task BindSectionAsync(SpatialActor actor, int floorId, int sectionId, CancellationToken ct)
    {
        var floor = await OwnedFloor(actor.PersonId, floorId, ct);
        if (!await db.Section.AnyAsync(x => x.Id == sectionId && !x.IsDeleted, ct)) throw new SpatialValidationException("Section not found.");
        if (!await db.SpatialFloorSections.AnyAsync(x => x.FloorId == floorId && x.SectionId == sectionId && !x.IsDeleted, ct))
            db.SpatialFloorSections.Add(new SpatialFloorSection { PersonId = actor.PersonId, CreatedBy = actor.PersonId, FloorId = floorId, SectionId = sectionId });
        await db.SaveChangesAsync(ct);
        await Emit(actor.PersonId, floor.Building.PropertyId, "SectionBinding", sectionId, "Created");
    }

    public async Task BindRoomAsync(SpatialActor actor, int floorId, int roomId, int? sectionId, CancellationToken ct)
    {
        var floor = await OwnedFloor(actor.PersonId, floorId, ct);
        var room = await db.Room.FirstOrDefaultAsync(x => x.Id == roomId && !x.IsDeleted, ct) ?? throw new SpatialValidationException("Room not found.");
        if (sectionId.HasValue)
        {
            var sectionBound = await db.SpatialFloorSections.AnyAsync(x => x.FloorId == floorId && x.SectionId == sectionId.Value && !x.IsDeleted, ct);
            if (!sectionBound) throw new SpatialValidationException("Section must belong to the selected floor before binding its room.");
            if (room.SectionId != sectionId.Value) throw new SpatialValidationException("Existing Room.SectionId must match the selected section.");
        }
        else if (room.SectionId.HasValue)
            throw new SpatialValidationException("A room with an existing SectionId cannot also be bound directly to the floor.");
        var old = await db.SpatialRoomBindings.FirstOrDefaultAsync(x => x.RoomId == roomId && !x.IsDeleted, ct);
        if (old != null) throw new SpatialValidationException("Room already has a spatial floor binding.");
        db.SpatialRoomBindings.Add(new SpatialRoomBinding { PersonId = actor.PersonId, CreatedBy = actor.PersonId, FloorId = floorId, RoomId = roomId, SectionId = sectionId });
        await db.SaveChangesAsync(ct);
        await Emit(actor.PersonId, floor.Building.PropertyId, "RoomBinding", roomId, "Created");
    }

    public async Task<SpatialGeometry> CreateGeometryAsync(SpatialActor actor, int propertyId, SpatialGeometryKind kind, string name, string geometryJson, double height, double thickness, CancellationToken ct)
    {
        await OwnedProperty(actor.PersonId, propertyId, ct);
        if (geometryJson.Length > 1_000_000) throw new SpatialValidationException("Geometry payload exceeds 1 MB.");
        using var parsed = JsonDocument.Parse(geometryJson);
        var g = new SpatialGeometry { PersonId = actor.PersonId, CreatedBy = actor.PersonId, PropertyId = propertyId, Kind = kind, Name = name.Trim(), GeometryJson = parsed.RootElement.GetRawText(), Height = height, Thickness = thickness };
        db.SpatialGeometries.Add(g);
        await db.SaveChangesAsync(ct);
        await Emit(actor.PersonId, propertyId, "Geometry", g.Id, "Created");
        return g;
    }

    public async Task<SpatialPlacement> CreatePlacementAsync(SpatialActor actor, SpatialPlacement p, CancellationToken ct)
    {
        await OwnedProperty(actor.PersonId, p.PropertyId, ct);
        if (!await targetValidator.ExistsAndBelongsToPersonAsync(p.TargetKind, p.TargetId, actor.PersonId, ct))
            throw new SpatialValidationException("Target facility/utility does not exist or is not accessible.");
        p.PersonId = actor.PersonId;
        p.CreatedBy = actor.PersonId;
        p.RotationW = p.RotationW == 0 && p.RotationX == 0 && p.RotationY == 0 && p.RotationZ == 0 ? 1 : p.RotationW;
        await geometryValidator.ValidatePlacementAsync(p, ct);
        var duplicate = await db.SpatialPlacements.AnyAsync(x => x.TargetKind == p.TargetKind && x.TargetId == p.TargetId && !x.IsDeleted, ct);
        if (duplicate) throw new SpatialValidationException("This existing device already has a spatial placement.");
        db.SpatialPlacements.Add(p);
        await db.SaveChangesAsync(ct);
        await Emit(actor.PersonId, p.PropertyId, "Placement", p.Id, "Created");
        return p;
    }

    public async Task DeletePlacementAsync(SpatialActor actor, int placementId, CancellationToken ct)
    {
        var p = await db.SpatialPlacements.FirstOrDefaultAsync(x => x.Id == placementId && x.PersonId == actor.PersonId && !x.IsDeleted, ct)
            ?? throw new SpatialValidationException("Placement not found.");
        p.IsDeleted = true; p.DeletedBy = actor.PersonId; p.DeletedOn = DateTime.UtcNow; p.Version++;
        await db.SaveChangesAsync(ct);
        await Emit(actor.PersonId, p.PropertyId, "Placement", p.Id, "Deleted");
        // Deliberately do not delete p.TargetId from the legacy facility table.
    }

    public async Task<object> GetSceneAsync(SpatialActor actor, int propertyId, CancellationToken ct)
    {
        var property = await OwnedProperty(actor.PersonId, propertyId, ct);
        var buildings = await db.SpatialBuildings.AsNoTracking().Where(x => x.PropertyId == propertyId && !x.IsDeleted)
            .Select(b => new { b.Id, b.Name, b.BaseElevation, Floors = b.Floors.Where(f => !f.IsDeleted).Select(f => new { f.Id, f.Name, f.LevelIndex, f.Elevation, f.DefaultCeilingHeight }).ToList() }).ToListAsync(ct);
        var geometry = await db.SpatialGeometries.AsNoTracking().Where(x => x.PropertyId == propertyId && !x.IsDeleted).ToListAsync(ct);
        var placements = await db.SpatialPlacements.AsNoTracking().Where(x => x.PropertyId == propertyId && !x.IsDeleted).ToListAsync(ct);
        // Conservative filtering using legacy IsHidden flags. Never serialize hidden room/section records.
        var hiddenRoomIds = await db.SpatialRoomBindings.Where(x => x.PersonId == actor.PersonId && !x.IsDeleted)
            .Join(db.Room.Where(r => r.IsHidden || r.IsDeleted), x => x.RoomId, r => r.Id, (x, r) => x.RoomId).ToListAsync(ct);
        var hiddenSectionIds = await db.SpatialFloorSections.Join(db.Section.Where(s => s.IsHidden || s.IsDeleted), x => x.SectionId, s => s.Id, (x, s) => x.SectionId).ToListAsync(ct);
        var hiddenGeometryIds = await db.SpatialGeometrySpaceLinks.Where(x =>
            (x.SpaceKind == SpatialNodeKind.Room && hiddenRoomIds.Contains(x.SpaceId)) ||
            (x.SpaceKind == SpatialNodeKind.Section && hiddenSectionIds.Contains(x.SpaceId))).Select(x => x.GeometryId).ToListAsync(ct);
        var visibleGeometry = geometry.Where(x => !hiddenGeometryIds.Contains(x.Id)).ToList();
        var visiblePlacements = placements.Where(x => !(x.ParentKind == SpatialNodeKind.Room && hiddenRoomIds.Contains(x.ParentId))
            && !(x.ParentKind == SpatialNodeKind.Section && hiddenSectionIds.Contains(x.ParentId))).ToList();
        return new { Property = new { property.Id, property.Name, property.CoordinateSystem, property.Version }, Buildings = buildings, Geometry = visibleGeometry, Placements = visiblePlacements };
    }

    private Task<SpatialProperty> OwnedProperty(int personId, int id, CancellationToken ct) =>
        db.SpatialProperties.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId && !x.IsDeleted, ct)
        ?? throw new SpatialValidationException("Property not found.");
    private Task<SpatialFloor> OwnedFloor(int personId, int id, CancellationToken ct) =>
        db.SpatialFloors.Include(x => x.Building).FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId && !x.IsDeleted, ct)
        ?? throw new SpatialValidationException("Floor not found.");
    private Task Emit(int personId, int propertyId, string type, int id, string operation) =>
        realtime.NotifyAll("SpatialSceneChanged", personId, new SpatialChange(propertyId, id, type, 1, operation));
}
