using Home_Security.Context;
using Home_Security.Spatial.Entities;
using Microsoft.EntityFrameworkCore;

namespace Home_Security.Spatial.Services;

/// <summary>
/// Validates that a referenced legacy record exists and is not soft-deleted.
/// Extend ownership checks for each facility using your established person/section authorization rules.
/// </summary>
public sealed class SpatialTargetValidator(HomeSecurityContext db) : ISpatialTargetValidator
{
    public async Task<bool> ExistsAndBelongsToPersonAsync(SpatialTargetKind kind, int id, int personId, CancellationToken ct)
    {
        return kind switch
        {
            SpatialTargetKind.Door => await db.Door.AnyAsync(x => x.Id == id && !x.IsDeleted, ct),
            SpatialTargetKind.Window => await db.Window.AnyAsync(x => x.Id == id && !x.IsDeleted, ct),
            SpatialTargetKind.Light => await db.Light.AnyAsync(x => x.Id == id && !x.IsDeleted, ct),
            SpatialTargetKind.Camera => await db.Camera.AnyAsync(x => x.Id == id && !x.IsDeleted, ct),
            SpatialTargetKind.Appliance => await db.Appliance.AnyAsync(x => x.Id == id && !x.IsDeleted, ct),
            SpatialTargetKind.External => await db.External.AnyAsync(x => x.Id == id && !x.IsDeleted, ct),
            SpatialTargetKind.Utility => await db.Utilities.AnyAsync(x => x.Id == id && x.PersonId == personId && !x.IsDeleted, ct),
            _ => false
        };
    }
}
