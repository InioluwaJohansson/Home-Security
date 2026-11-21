using Home_Security.Context;
using Microsoft.EntityFrameworkCore;

namespace Home_Security.Spatial;
public sealed class SpatialTargetValidator : ISpatialTargetValidator
{
    HomeSecurityContext db;
    public SpatialTargetValidator(HomeSecurityContext _db) => db = _db;
    public async Task<bool> ExistsAndBelongsToPersonAsync(SpatialTargetKind kind, int id, int personId)
    {
        return kind switch
        {
            SpatialTargetKind.Door => await db.Door.AnyAsync(x => x.Id == id && !x.IsDeleted),
            SpatialTargetKind.Window => await db.Window.AnyAsync(x => x.Id == id && !x.IsDeleted),
            SpatialTargetKind.Light => await db.Light.AnyAsync(x => x.Id == id && !x.IsDeleted),
            SpatialTargetKind.Camera => await db.Camera.AnyAsync(x => x.Id == id && !x.IsDeleted),
            SpatialTargetKind.Appliance => await db.Appliance.AnyAsync(x => x.Id == id && !x.IsDeleted),
            SpatialTargetKind.External => await db.External.AnyAsync(x => x.Id == id && !x.IsDeleted),
            SpatialTargetKind.Utility => await db.Utilities.AnyAsync(x => x.Id == id && x.PersonId == personId && !x.IsDeleted),
            _ => false
        };
    }
}
