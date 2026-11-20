# Pre-migration and pre-production review

- [ ] Merge DbSet/configuration additions into existing `HomeSecurityContext`; do not overwrite it.
- [ ] Check the actual Pomelo EF Core package version and target framework.
- [ ] Run `dotnet build`.
- [ ] Generate migration `AddSpatial3D` and inspect every FK, index, column type and delete behavior.
- [ ] Run migration on a disposable database first.
- [ ] Confirm existing table names (`Room`, `Section`, `Door`, `Window`, `Light`, `Camera`, `Appliance`, `External`, `Utilities`) match actual EF mappings.
- [ ] Extend `SpatialTargetValidator` to verify that every facility belongs to the authenticated person's authorized property/sections. Current facilities have different ownership patterns and cannot be assumed to be person-owned by a simple PersonId field.
- [ ] Implement a real geometry-kernel adapter for polygon/mesh containment, wall openings, curved surfaces and full-volume placement validation. The default validator is intentionally conservative and does not claim exact collision detection.
- [ ] Add authorization rules for Owner/Wife/Child/Guest etc. based on the actual Role enum and `CompareRole` policy. Do not infer role permissions in the frontend.
- [ ] Add access-rule precedence and test hidden-room/section filtering before exposing scene APIs.
- [ ] Ensure person group membership is established: current hub requires `AddPersonRole(token)` after connection.
- [ ] Add API tests for cross-person IDs, hidden room leaks, irrigation placement at Property root, Gate on non-boundary wall, and placement deletion preserving underlying device.
- [ ] Add endpoints/DTOs for editing/deleting geometry, openings, structures, light fixtures, access rules and version conflict handling before claiming full CRUD.
