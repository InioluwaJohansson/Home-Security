namespace Home_Security;

/// <summary>Coordinate and geometry classifications used by the spatial model.</summary>
public enum SpatialGeometryType
{
    Point = 1,
    LineString,
    Polygon,
    MultiPolygon,
    Mesh,
    Box,
    Cylinder,
    Custom
}

public enum SpatialGeometryOwnerType
{
    Property = 1,
    Building,
    Floor,
    FloorSection,
    Room,
    ExteriorArea,
    FloorSurface,
    Wall,
    WallSegment,
    Opening,
    Staircase,
    StairFlight,
    StairLanding,
    Roof,
    RoofEdge,
    Structure
}

public enum SpatialAnchorType
{
    Floor = 1,
    Section,
    Room,
    Wall,
    WallSegment,
    FloorSurface,
    ExteriorArea,
    Structure,
    Staircase,
    Roof
}

public enum SpatialSurfaceRole
{
    Ground = 1,
    Ceiling,
    RoofDeck,
    Intermediate
}

public enum SpatialSurfaceType
{
    Slab = 1,
    Concrete,
    Timber,
    Glass,
    Metal,
    Roof,
    Other
}

public enum SpatialWallType
{
    Interior = 1,
    Exterior,
    Retaining,
    Partition,
    Glass,
    Boundary,
    Parapet,
    Other
}

public enum SpatialOpeningType
{
    Door = 1,
    Window,
    Gate,
    Passage,
    Ventilation,
    Other
}

public enum SpatialMountingType
{
    Floor = 1,
    Wall,
    Ceiling,
    Surface,
    Freestanding
}

public enum SpatialStructureType
{
    Scaffold = 1,
    Column,
    Beam,
    Pillar,
    TankStand,
    Pergola,
    Other
}

public enum SpatialRoofType
{
    Flat = 1,
    Sloped,
    Hip,
    Gable,
    Custom
}

public enum SpatialRoofEdgeType
{
    Straight = 1,
    Slanted,
    Beveled,
    Chamfered,
    Curved,
    Custom
}

public enum SpatialUtilityAnchorType
{
    Floor = 1,
    Section,
    Room,
    Wall,
    CeilingSurface,
    FloorSurface,
    ExteriorArea,
    Structure
}
