# Cursor Rules — HeritageVault (HV)

## Authoritative scope
When generating ANY code, ALWAYS follow docs/scope.md as the single source of truth for:
- Entities, relationships, and lifecycle rules
- Enums and allowed values
- API behavior and idempotency
- Delete behavior (soft vs hard)
- Business rules and constraints

If there is any conflict between a prompt and docs/scope.md, docs/scope.md MUST win.

---

## Architecture and style
- Solution name prefix MUST be: HV
- The solution MUST have exactly 3 layers:
  - HV.WebAPI
  - HV.BLL
  - HV.DAL
- Dependency direction MUST be:
  WebAPI → BLL → DAL
- Entities MUST exist only in DAL
- All entities MUST inherit from BaseEntity

---

## Fixed interfaces and classes (mandatory reuse)
The following types are authoritative and MUST be reused exactly:
- BaseEntity
- IRepository<TEntity>
- Repository<TEntity>
- IUnitOfWork
- UnitOfWork
- CustomExceptionBase
- NotFoundException
- ErrorResponse
- CustomExceptionFilterAttribute

MUST NOT redefine or replace these types.

---

## Data access rules
- MUST use IRepository<TEntity> and IUnitOfWork
- MUST NOT create custom repositories
- IRepository<TEntity> MUST NOT call SaveChangesAsync
- IUnitOfWork is the only component allowed to:
  - call SaveChangesAsync
  - manage transactions

---

## Async rules
- Use async only where EF Core provides async APIs
- MUST NOT fake async with Task.CompletedTask
- Add/Insert may be async
- Update/Delete may be synchronous

---

## Business logic placement
- ALL business rules MUST be implemented in BLL services
- Controllers MUST NOT contain business logic
- Validators handle shape and ranges only
- Services handle:
  - idempotency
  - uniqueness
  - soft delete rules
  - relationship integrity

---

## API contracts
- Controllers MUST use DTOs only
- MUST NOT accept or return entities
- DTOs MUST be C# records
- Query parameters MUST be wrapped in query DTOs

---

## Mapping rules
- Mapping MUST be manual
- AutoMapper or reflection-based mapping is forbidden
- Mapping code MUST live in BLL

### .NET 10 extension blocks (mandatory)
- Use extension blocks introduced in .NET 10 for mapping extensions and collection helpers where appropriate.
- Prefer grouping related mapping extensions into a single extension block per entity/feature.

### Collection mapping (mandatory)
- MUST NOT inline collection mapping like:
  - `return result.Select(x => x.ToDto()).ToList();`
- MUST create and use dedicated collection mapping extensions, e.g.:
  - `IEnumerable<CountryListItemDto> ToListItemDtos(this IEnumerable<Country> items)`
  - `List<CountryListItemDto> ToListItemDtoList(this IEnumerable<Country> items)`
- Controllers and services MUST call the collection mapping extension.

---

## Validation
- Every request DTO and query DTO MUST have a FluentValidation validator
- Validators MUST NOT query the database
- Database/state validation belongs in services

---

## Error handling
- Not found → throw NotFoundException → HTTP 404
- Expected business errors → throw CustomExceptionBase → HTTP 400
- Duplicate create requests MUST be idempotent (return existing entity)
- MUST NOT return HTTP 409 anywhere
- Unexpected errors → HTTP 500

---

## Exception handling
- CustomExceptionFilterAttribute MUST be registered globally
- Controllers MUST NOT use try/catch for application errors

---

## Coding style

### General
- Use primary constructors where possible
- Use readonly fields initialized from constructor parameters
- Always use var when type is obvious
- Prefer LINQ where it improves clarity
- Prefer `??` and `??=` where it improves clarity and reduces branching

### Null checks (mandatory)
- MUST use pattern matching null checks:
  - Use `is null` instead of `== null`
  - Use `is not null` instead of `!= null`
- For nullable value types, MUST prefer:
  - `if (x is not null)` instead of `if (x.HasValue)`
  - `if (x is null)` instead of `if (!x.HasValue)`
- MUST prefer direct null-pattern checks over `{ }` property patterns for null validation:
  - Prefer: `if (excludeId is not null)`
  - Avoid: `if (excludeId is { } id)`
- Access `.Value` only when the nullable was proven non-null, or use `??`/`??=`.

### Pattern matching (required when suitable)
Use pattern matching to simplify logic where it improves clarity, e.g.:
- `if (entity is null) throw ...`
- `if (request is { TagIds.Count: > 0 }) ...`
- `if (value is >= 1 and <= 100) ...`
- `switch` expressions for simple mapping/translation when appropriate (do not split arms across multiple lines)

### Formatting
- if statements MUST NOT use braces
- Controlled statement MUST be on the next line
- Switch expressions MUST NOT split arms across lines

---

## No scope creep
MUST NOT add:
- authentication or authorization
- users or roles
- payments or financial logic
- background jobs
- file upload logic

unless docs/scope.md is explicitly updated.
