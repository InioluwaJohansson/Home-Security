# Home Security spatial entities (fresh model)

All entities use `namespace Home_Security;` and inherit from the existing
`Home_Security.Contracts.AuditableEntity` unless they are enums.

## Integration notes
- Copy the `.cs` files into the backend's `Spatial` folder.
- These are entity definitions only. No DbSet registrations, EF Core fluent
  configurations, migrations, repositories, DTOs, validators, services, or
  controllers are included.
- Existing device records remain authoritative. `SpatialPlacement`,
  `SpatialOpening`, `SpatialLightFixtureMember`, and `SpatialUtilityPlacement`
  reference their existing integer IDs.
- Polymorphic references (`FacilityType + FacilityId`, `AnchorType + AnchorId`,
  `OwnerType + OwnerId`, `SpaceType + SpaceId`) require service-level validation
  and cannot be represented as ordinary EF foreign keys.
- A physical `SpatialFloorSurface` can be assigned as `Ceiling` for one floor
  and `Ground` for the floor above.
- A `SpatialWall` can span floors through `SpatialWallFloor`.
- Cameras can be mounted to walls or ceiling surfaces using `SpatialPlacement`.
  For ceiling mounting, use an anchor to a `SpatialFloorSurface` whose assignment
  role is `Ceiling`.
- Hidden room/section geometry and associated placements must be filtered on the
  server before returning scene data. A hidden entrance should be represented as
  a solid wall in the filtered scene; do not return its opening/device metadata.
- `SpatialUtilityAnchorType` intentionally has no `Property` value. A utility
  must be anchored to a real location inside the property.
- `SpatialRoomBinding.SpatialFloorSectionId` is nullable to support rooms directly
  on a floor.
- Geometry uses X/Y/Z doubles in one project-wide coordinate convention. Define
  the exact axis orientation, unit, and origin before accepting production data.

## Important follow-up before EF configuration
1. Confirm your preferred coordinate convention (for example X=east, Y=north,
   Z=up; metres; origin at a fixed property corner).
2. Add database constraints/indexes for unique room/floor bindings, section/floor
   bindings, fixture membership, and one roof per building if those are desired.
3. Validate polymorphic IDs, opening placement, door gate boundary-only rules,
   irrigation placement rules, hidden access, and geometry containment in services.
