using Microsoft.EntityFrameworkCore;
using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Entities.Chats;
using Home_Security.Entities.Calls;
using Home_Security.Spatial;

namespace Home_Security.Context;
public class HomeSecurityContext: DbContext
{
    public HomeSecurityContext(DbContextOptions<HomeSecurityContext> optionsBuilder): base(optionsBuilder)
    {
    }
    //User
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    //Facilities
    public DbSet<Section> Section { get; set; }
    public DbSet<Room> Room { get; set; }
    public DbSet<Door> Door { get; set; }
    public DbSet<Window> Window { get; set; }
    public DbSet<Light> Light { get; set; }
    public DbSet<Camera> Camera { get; set; }
    public DbSet<Appliance> Appliance { get; set; }
    public DbSet<External> External { get; set; }


    //Operations
    public DbSet<Entities.Action> Action { get; set; }
    public DbSet<Utilities> Utilities { get; set; }
    public DbSet<Logs> Logs { get; set; }
    public DbSet<Recording> Recording { get; set; }

    //Hardware
    public DbSet<Hardware> Hardware { get; set; }

    //Person
    public DbSet<Person> Person { get; set; }
    public DbSet<PersonDetails> PersonDetails { get; set; }
    public DbSet<FingerPrint> FingerPrint { get; set; }

    //Contact Profiles
    public DbSet<Address> Address { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public DbSet<ContactCategory> ContactCategory { get; set; }
    public DbSet<ContactDetails> ContactDetails { get; set; }

    //Token
    public DbSet<Token> Tokens { get; set; }

    //Chats
    public DbSet<Chat> Chat { get; set; }
    public DbSet<ChatParticipant> ChatParticipant { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<MessageAttachment> MessageAttachment { get; set; }

    //Calls
    public DbSet<Call> Calls { get; set; }
    public DbSet<CallParticipant> CallParticipants { get; set; }

    // Spatial: Property and Building
    public DbSet<SpatialProperty> SpatialProperties { get; set; }
    public DbSet<SpatialBuilding> SpatialBuildings { get; set; }

    // Spatial: Floors and Spaces
    public DbSet<SpatialFloor> SpatialFloors { get; set; }
    public DbSet<SpatialFloorSection> SpatialFloorSections { get; set; }
    public DbSet<SpatialRoomBinding> SpatialRoomBindings { get; set; }
    public DbSet<SpatialExteriorArea> SpatialExteriorAreas { get; set; }

    // Spatial: Geometry
    public DbSet<SpatialGeometry> SpatialGeometries { get; set; }
    public DbSet<SpatialGeometryPoint> SpatialGeometryPoints { get; set; }
    public DbSet<SpatialGeometrySpaceLink> SpatialGeometrySpaceLinks { get; set; }

    // Spatial: Surfaces and Walls
    public DbSet<SpatialFloorSurface> SpatialFloorSurfaces { get; set; }
    public DbSet<SpatialFloorSurfaceAssignment> SpatialFloorSurfaceAssignments { get; set; }
    public DbSet<SpatialWall> SpatialWalls { get; set; }
    public DbSet<SpatialWallFloor> SpatialWallFloors { get; set; }
    public DbSet<SpatialWallSegment> SpatialWallSegments { get; set; }

    // Spatial: Openings and Property Boundary
    public DbSet<SpatialOpening> SpatialOpenings { get; set; }
    public DbSet<SpatialBoundarySegment> SpatialBoundarySegments { get; set; }

    // Spatial: Device Placement
    public DbSet<SpatialPlacement> SpatialPlacements { get; set; }
    public DbSet<SpatialLightFixture> SpatialLightFixtures { get; set; }
    public DbSet<SpatialLightFixtureMember> SpatialLightFixtureMembers { get; set; }

    // Spatial: Stairs and Roof
    public DbSet<SpatialStaircase> SpatialStaircases { get; set; }
    public DbSet<SpatialStairFlight> SpatialStairFlights { get; set; }
    public DbSet<SpatialStairLanding> SpatialStairLandings { get; set; }
    public DbSet<SpatialRoof> SpatialRoofs { get; set; }
    public DbSet<SpatialRoofEdge> SpatialRoofEdges { get; set; }

    // Spatial: Structures
    public DbSet<SpatialStructure> SpatialStructures { get; set; }
    public DbSet<SpatialStructureSupport> SpatialStructureSupports { get; set; }

    // Spatial: Utilities and Access
    public DbSet<SpatialUtilityPlacement> SpatialUtilityPlacements { get; set; }
    public DbSet<SpatialAccessRule> SpatialAccessRules { get; set; }
}
