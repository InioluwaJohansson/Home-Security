using Home_Security.Context;
using Home_Security.Spatial.Entities;
using Microsoft.EntityFrameworkCore;

namespace Home_Security.Spatial.Services;

/// <summary>
/// Conservative structural validation. Exact mesh/solid containment belongs in a tested geometry-kernel adapter.
/// This implementation deliberately does not claim that an AABB test is exact containment.
/// </summary>
public sealed class SpatialGeometryValidator(HomeSecurityContext db) : ISpatialGeometryValidator
{
    public async Task ValidatePlacementAsync(SpatialPlacement p, CancellationToken ct)
    {
        if (p.Width < 0 || p.Height < 0 || p.Depth < 0)
            throw new SpatialValidationException("Placement dimensions cannot be negative.");
        if (p.ScaleX <= 0 || p.ScaleY <= 0 || p.ScaleZ <= 0)
            throw new SpatialValidationException("Placement scale must be greater than zero.");
        var property = await db.SpatialProperties.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == p.PropertyId && x.PersonId == p.PersonId && !x.IsDeleted, ct);
        if (property is null) throw new SpatialValidationException("Property not found.");
        if (p.TargetKind == SpatialTargetKind.Utility)
        {
            // Property root is not a valid irrigation target; all utility placements must target a contained node/structure.
            if (p.ParentKind == SpatialNodeKind.Property)
            {
                var irrigation = await db.Utilities.AsNoTracking()
                    .AnyAsync(x => x.Id == p.TargetId && x.PersonId == p.PersonId && x.UtilityType == Home_Security.Models.Enums.UtilityType.Irrigation, ct);
                if (irrigation) throw new SpatialValidationException("Irrigation must be placed on a contained area, floor, room, section or structure, not directly on Property.");
            }
        }
        if (p.AttachmentKind == SpatialAttachmentKind.OpeningMounted && p.OpeningId == null)
            throw new SpatialValidationException("OpeningMounted placement requires OpeningId.");
        if (p.AttachmentKind is SpatialAttachmentKind.WallMounted or SpatialAttachmentKind.CeilingMounted or SpatialAttachmentKind.FloorMounted or SpatialAttachmentKind.StructureMounted && p.HostGeometryId == null)
            throw new SpatialValidationException("Surface/structure-mounted placement requires HostGeometryId.");
    }
}
