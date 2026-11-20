using Home_Security.Spatial.Entities;
using Microsoft.EntityFrameworkCore;

namespace Home_Security.Spatial.Configuration;

public static class SpatialModelConfiguration
{
    public static void ConfigureSpatialModel(this ModelBuilder b)
    {
        b.Entity<SpatialProperty>().ToTable("SpatialProperties").HasIndex(x => new { x.PersonId, x.IsDeleted });
        b.Entity<SpatialBuilding>().ToTable("SpatialBuildings").HasOne(x => x.Property).WithMany(x => x.Buildings).HasForeignKey(x => x.PropertyId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<SpatialFloor>().ToTable("SpatialFloors").HasOne(x => x.Building).WithMany(x => x.Floors).HasForeignKey(x => x.BuildingId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<SpatialFloorSection>().ToTable("SpatialFloorSections").HasIndex(x => new { x.FloorId, x.SectionId }).IsUnique();
        b.Entity<SpatialFloorSection>().HasOne(x => x.Floor).WithMany(x => x.Sections).HasForeignKey(x => x.FloorId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<SpatialFloorSection>().HasOne<Home_Security.Entities.Section>().WithMany().HasForeignKey(x => x.SectionId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<SpatialRoomBinding>().ToTable("SpatialRoomBindings").HasIndex(x => x.RoomId).IsUnique();
        b.Entity<SpatialRoomBinding>().HasOne(x => x.Floor).WithMany(x => x.DirectRooms).HasForeignKey(x => x.FloorId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<SpatialRoomBinding>().HasOne<Home_Security.Entities.Room>().WithMany().HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<SpatialExteriorArea>().ToTable("SpatialExteriorAreas").HasOne(x => x.Property).WithMany(x => x.ExteriorAreas).HasForeignKey(x => x.PropertyId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<SpatialGeometry>().ToTable("SpatialGeometries").HasOne(x => x.Property).WithMany(x => x.Geometries).HasForeignKey(x => x.PropertyId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<SpatialGeometry>().HasIndex(x => new { x.PropertyId, x.Kind });
        b.Entity<SpatialGeometryPoint>().ToTable("SpatialGeometryPoints").HasOne(x => x.Geometry).WithMany(x => x.Points).HasForeignKey(x => x.GeometryId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<SpatialGeometrySpaceLink>().ToTable("SpatialGeometrySpaceLinks").HasOne(x => x.Geometry).WithMany(x => x.Spaces).HasForeignKey(x => x.GeometryId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<SpatialOpening>().ToTable("SpatialOpenings").HasOne(x => x.HostGeometry).WithMany().HasForeignKey(x => x.HostGeometryId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<SpatialPlacement>().ToTable("SpatialPlacements").HasIndex(x => new { x.TargetKind, x.TargetId }).IsUnique();
        b.Entity<SpatialPlacement>().HasIndex(x => new { x.PropertyId, x.ParentKind, x.ParentId });
        b.Entity<SpatialStructure>().ToTable("SpatialStructures").HasMany(x => x.Supports).WithOne(x => x.Structure).HasForeignKey(x => x.StructureId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<SpatialStructureSupport>().ToTable("SpatialStructureSupports").HasIndex(x => new { x.StructureId, x.PlacementId }).IsUnique();
        b.Entity<SpatialLightFixture>().ToTable("SpatialLightFixtures").HasMany(x => x.Members).WithOne(x => x.Fixture).HasForeignKey(x => x.FixtureId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<SpatialLightFixtureMember>().ToTable("SpatialLightFixtureMembers").HasIndex(x => x.LightId).IsUnique();
        b.Entity<SpatialLightFixtureMember>().HasOne<Home_Security.Entities.Light>().WithMany().HasForeignKey(x => x.LightId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<SpatialAccessRule>().ToTable("SpatialAccessRules").HasIndex(x => new { x.PropertyId, x.TargetKind, x.TargetId, x.UserId, x.RoleName });
        foreach (var entity in b.Model.GetEntityTypes().Where(x => typeof(SpatialAuditableEntity).IsAssignableFrom(x.ClrType)))
        {
            // common owner/soft-delete indexes are declared below per entity through EF metadata convention
            b.Entity(entity.ClrType).HasIndex(nameof(SpatialAuditableEntity.PersonId), nameof(SpatialAuditableEntity.IsDeleted));
        }
    }
}
