# HanssonHub Spatial 3D Backend Extension

This is an integration package for the existing `Home-Security` ASP.NET Core Web API.
It is **not a replacement application**. It targets the repository structure inspected during preparation:

- `Home_Security.Context.HomeSecurityContext`
- `Home_Security.Authentication.IJWTAuthentication`
- `Home_Security.Interfaces.Controls.IAuthControl`
- `Home_Security.RealTimeServices.IRealtimeNotificationService`
- `Home_Security.Hubs.HomeSecurityHub`
- MySQL 8.0.42 through EF Core / Pomelo
- Existing `Person`, `Room`, `Section`, `Door`, `Window`, `Light`, `Camera`, `Appliance`, `External`, and `Utilities` entities

## Important integration notes

1. Copy `Backend/` into the existing backend project, preserving folders.
2. Merge the `HomeSecurityContext` additions from `Integration/HomeSecurityContext.additions.cs.txt` into the existing context. Do not replace the existing context.
3. Merge the DI and middleware additions from `Integration/Program.additions.cs.txt` into the existing `Program.cs`.
4. Do not add another JWT scheme, login endpoint, SignalR hub, or token store.
5. Run EF Core migrations against the existing `HomeSecurityContext` after reviewing the generated migration:
   ```powershell
   dotnet ef migrations add AddSpatial3D --context HomeSecurityContext
   dotnet ef database update --context HomeSecurityContext
   ```
6. Build and test in a branch before deploying.

## Authentication contract

The existing login flow remains the only token issuer. The repository's current login response uses a `token` field and its existing JWT claims include `PersonId` and `AuthorizationCode`. The frontend must use the token returned by the existing login API as:

`Authorization: Bearer <token>`

Spatial controllers have `[Authorize]` and additionally call the existing `IJWTAuthentication.GetUserFromToken(...)` and `IAuthControl.GetAuthDetails(personId, authorizationCode)` checks, following the existing controllers' pattern. The authenticated person is always derived from the validated token, never from a client-supplied PersonId.

Existing login route observed in the repository: `POST /Home_Security/User/Login`. Its exact request DTO must be taken from the existing frontend/login implementation or Swagger; this package deliberately does not invent a login JSON schema.

## Real-time contract

The existing service is `RealtimeNotificationService`, backed by `IHubContext<HomeSecurityHub>`. Spatial changes use:
`IRealtimeNotificationService.NotifyAll(eventName, personId, payload)`

The existing implementation sends to the SignalR group named with the person ID. The frontend must connect to `/HomeSecurityHub` using SignalR `accessTokenFactory`, then invoke the existing `AddPersonRole(token)` method after connection so it joins its person group. Do not create a new hub.

Events emitted:
- `SpatialSceneChanged`
- `SpatialPlacementChanged`
- `SpatialVisibilityChanged`

The server sends only the changed resource summary, not an unrestricted complete scene. Full scene reads are still authorized HTTP requests.

## Data design

- `SpatialProperty` is the property-scoped root; a scene is owned by the authenticated `PersonId`.
- Building -> Floor; floor-to-section is a link table because existing `Section` is reused.
- Existing `Room` is reused through `SpatialRoomBinding`; `Room.SectionId` remains nullable. A room can be assigned to a section/floor or directly to a floor, but not both simultaneously.
- Existing `Section` and `Room` records remain authoritative for their names and `IsHidden` flags.
- Physical geometry is independent of room/floor associations. Shared walls and shared slabs have one physical geometry identity.
- Geometry is stored as typed JSON plus normalized point rows for editable curves/polygons. JSON is validated and size-limited; it is not trusted as executable input.
- Device/utility spatial placement uses an explicit `TargetKind` + `TargetId` reference. SQL cannot enforce a conventional FK across multiple existing facility tables, so `ISpatialTargetValidator` is the mandatory validation seam. The included validator validates ownership and existence for supported targets; extend it for your exact facility ownership rules before production.
- Spatial placement deletion never deletes the underlying facility/utility.
- Irrigation cannot be attached directly to the Property root. It must target a contained spatial node/structure within the property boundary. Existing `UtilityComponent` enum remains unchanged; no irrigation component values are fabricated.
- Room/section hidden state is checked before scene data is returned. This package uses existing `IsHidden` as a conservative deny rule; role/user overrides are an extension point and must be added before relying on complex per-user exceptions.
- All measurements are metres; right-handed coordinates, Y-up; rotations are quaternions.

## Geometry validation limitation

The included validation enforces hierarchy, owner, target type, dimensions, property scope and attachment references. Exact point-in-solid / mesh-volume containment and curved-wall collision checks require a geometry kernel or a carefully tested computational-geometry implementation. The service exposes `ISpatialGeometryValidator` for that implementation. The default validator intentionally rejects unsupported free-form containment claims instead of pretending an AABB check is exact.

## Known project-specific follow-up

The GitHub repository's current `RealtimeNotificationService.NotifyAll(eventName, personId, payload)` targets a group named by person ID, while `HomeSecurityHub.OnConnectedAsync` does not automatically add that group. Existing `AddPersonRole(token)` does. Ensure the frontend invokes it after connection; alternatively, safely add the authenticated PersonId group in `OnConnectedAsync` after checking current auth-cache policy.
