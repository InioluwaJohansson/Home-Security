using Home_Security.Contracts;
using Home_Security.Models.Enums;

namespace Home_Security.Spatial.Entities;

public enum SpatialNodeKind { Property = 1, Building = 2, Floor = 3, Section = 4, Room = 5, ExteriorArea = 6, Structure = 7 }
public enum SpatialGeometryKind { RoomOutline = 1, Wall = 2, WallSegment = 3, Slab = 4, RoofSurface = 5, Boundary = 6, Pool = 7, GlassPanel = 8, StairFlight = 9, StairLanding = 10, Void = 11, Generic = 12 }
public enum SpatialTargetKind { Door = 1, Window = 2, Light = 3, Camera = 4, Appliance = 5, External = 6, Utility = 7 }
public enum SpatialAttachmentKind { Free = 1, WallMounted = 2, CeilingMounted = 3, FloorMounted = 4, OpeningMounted = 5, StructureMounted = 6, Suspended = 7 }
public enum SpatialOpeningKind { Door = 1, Window = 2, Gate = 3, Generic = 4 }
public enum SpatialVisibilityEffect { HideNodeAndDescendants = 1, HidePlacement = 2, HideOpening = 3 }

public abstract class SpatialAuditableEntity : AuditableEntity
{
    public int PersonId { get; set; }
    public int Version { get; set; } = 1;
}

public sealed class SpatialProperty : SpatialAuditableEntity
{
    public string Name { get; set; } = "My Property";
    public string CoordinateSystem { get; set; } = "RightHanded-YUp-Metres";
    public double? BoundaryArea { get; set; }
    public ICollection<SpatialBuilding> Buildings { get; set; } = new List<SpatialBuilding>();
    public ICollection<SpatialExteriorArea> ExteriorAreas { get; set; } = new List<SpatialExteriorArea>();
    public ICollection<SpatialGeometry> Geometries { get; set; } = new List<SpatialGeometry>();
}
public sealed class SpatialBuilding : SpatialAuditableEntity
{
    public int PropertyId { get; set; }
    public SpatialProperty Property { get; set; } = null!;
    public string Name { get; set; } = "";
    public double BaseElevation { get; set; }
    public ICollection<SpatialFloor> Floors { get; set; } = new List<SpatialFloor>();
}
public sealed class SpatialFloor : SpatialAuditableEntity
{
    public int BuildingId { get; set; }
    public SpatialBuilding Building { get; set; } = null!;
    public string Name { get; set; } = "";
    public int LevelIndex { get; set; }
    public double Elevation { get; set; }
    public double? DefaultCeilingHeight { get; set; }
    public ICollection<SpatialFloorSection> Sections { get; set; } = new List<SpatialFloorSection>();
    public ICollection<SpatialRoomBinding> DirectRooms { get; set; } = new List<SpatialRoomBinding>();
}
public sealed class SpatialFloorSection : SpatialAuditableEntity
{
    public int FloorId { get; set; }
    public SpatialFloor Floor { get; set; } = null!;
    public int SectionId { get; set; }
}
public sealed class SpatialRoomBinding : SpatialAuditableEntity
{
    public int FloorId { get; set; }
    public SpatialFloor Floor { get; set; } = null!;
    public int RoomId { get; set; }
    public int? SectionId { get; set; }
    public bool IsDirectFloorRoom => SectionId == null;
}
public sealed class SpatialExteriorArea : SpatialAuditableEntity
{
    public int PropertyId { get; set; }
    public SpatialProperty Property { get; set; } = null!;
    public string Name { get; set; } = "";
    public string AreaKind { get; set; } = "Garden";
}
public sealed class SpatialGeometry : SpatialAuditableEntity
{
    public int PropertyId { get; set; }
    public SpatialProperty Property { get; set; } = null!;
    public SpatialGeometryKind Kind { get; set; }
    public string Name { get; set; } = "";
    public string GeometryJson { get; set; } = "{}";
    public double Thickness { get; set; }
    public double Height { get; set; }
    public string? MaterialCode { get; set; }
    public bool IsGenerated { get; set; }
    public int? SourceOutlineGeometryId { get; set; }
    public int? HostGeometryId { get; set; }
    public ICollection<SpatialGeometryPoint> Points { get; set; } = new List<SpatialGeometryPoint>();
    public ICollection<SpatialGeometrySpaceLink> Spaces { get; set; } = new List<SpatialGeometrySpaceLink>();
}
public sealed class SpatialGeometryPoint : SpatialAuditableEntity
{
    public int GeometryId { get; set; }
    public SpatialGeometry Geometry { get; set; } = null!;
    public int Sequence { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public string PointKind { get; set; } = "Vertex";
    public double? ControlX { get; set; }
    public double? ControlY { get; set; }
    public double? ControlZ { get; set; }
}
public sealed class SpatialGeometrySpaceLink : SpatialAuditableEntity
{
    public int GeometryId { get; set; }
    public SpatialGeometry Geometry { get; set; } = null!;
    public SpatialNodeKind SpaceKind { get; set; }
    public int SpaceId { get; set; }
    public string Relationship { get; set; } = "Bounds";
}
public sealed class SpatialOpening : SpatialAuditableEntity
{
    public int PropertyId { get; set; }
    public int HostGeometryId { get; set; }
    public SpatialGeometry HostGeometry { get; set; } = null!;
    public SpatialOpeningKind Kind { get; set; }
    public double Offset { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double SillHeight { get; set; }
    public string? FrameMaterial { get; set; }
    public string? Operation { get; set; }
    public string? PanelConfigurationJson { get; set; }
}
public sealed class SpatialPlacement : SpatialAuditableEntity
{
    public int PropertyId { get; set; }
    public SpatialTargetKind TargetKind { get; set; }
    public int TargetId { get; set; }
    public SpatialNodeKind ParentKind { get; set; }
    public int ParentId { get; set; }
    public SpatialAttachmentKind AttachmentKind { get; set; }
    public int? HostGeometryId { get; set; }
    public int? OpeningId { get; set; }
    public int? SupportingPlacementId { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public double PositionZ { get; set; }
    public double RotationX { get; set; }
    public double RotationY { get; set; }
    public double RotationZ { get; set; }
    public double RotationW { get; set; } = 1;
    public double ScaleX { get; set; } = 1;
    public double ScaleY { get; set; } = 1;
    public double ScaleZ { get; set; } = 1;
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    public string? AssetCode { get; set; }
    public string? AnchorJson { get; set; }
}
public sealed class SpatialStructure : SpatialAuditableEntity
{
    public int PropertyId { get; set; }
    public string Name { get; set; } = "";
    public string StructureType { get; set; } = "Scaffold";
    public string GeometryJson { get; set; } = "{}";
    public ICollection<SpatialStructureSupport> Supports { get; set; } = new List<SpatialStructureSupport>();
}
public sealed class SpatialStructureSupport : SpatialAuditableEntity
{
    public int StructureId { get; set; }
    public SpatialStructure Structure { get; set; } = null!;
    public int PlacementId { get; set; }
    public string SupportPointCode { get; set; } = "";
    public double MaxLoadKg { get; set; }
}
public sealed class SpatialLightFixture : SpatialAuditableEntity
{
    public int PropertyId { get; set; }
    public string Name { get; set; } = "";
    public string Arrangement { get; set; } = "Individual";
    public string MountingStyle { get; set; } = "Ceiling";
    public string LayoutJson { get; set; } = "{}";
    public ICollection<SpatialLightFixtureMember> Members { get; set; } = new List<SpatialLightFixtureMember>();
}
public sealed class SpatialLightFixtureMember : SpatialAuditableEntity
{
    public int FixtureId { get; set; }
    public SpatialLightFixture Fixture { get; set; } = null!;
    public int LightId { get; set; }
    public int Sequence { get; set; }
    public double OffsetX { get; set; }
    public double OffsetY { get; set; }
    public double OffsetZ { get; set; }
}
public sealed class SpatialAccessRule : SpatialAuditableEntity
{
    public int PropertyId { get; set; }
    public SpatialNodeKind TargetKind { get; set; }
    public int TargetId { get; set; }
    public int? UserId { get; set; }
    public string? RoleName { get; set; }
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public int Priority { get; set; }
}
