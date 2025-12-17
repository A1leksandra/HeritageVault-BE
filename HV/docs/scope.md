# HeritageVault — Project Scope

## Domain summary
HeritageVault is a public registry system for historical landmarks.  
The system stores and exposes information about landmarks, their locations, their historical condition, legal protection, accessibility, and cultural classifications.

The platform is designed as an open, read-only friendly registry:
- no authentication
- no ownership
- no user-specific behavior

Primary use cases:
- maintain a clean, structured registry of countries, regions, cities, and landmarks
- classify landmarks using reusable tags
- browse and filter landmarks by location, condition, protection, accessibility, and tags
- safely retire (soft-delete) obsolete reference data without losing historical context

The system is intentionally simple and extensible.

---

## Core concepts (business view)

### Location hierarchy
- **Country** → **Region** → **City**
- Regions are optional for cities.
- Landmarks always belong to exactly one city.

### Landmark classification
Each landmark has:
- exactly one **Protection Status** (legal)
- exactly one **Physical Condition** (state)
- exactly one **Accessibility Status** (public access)
- zero or more **Landmark Tags** (descriptive labels)

---

## Enums (business meanings)

### ProtectionStatus
Legal or administrative protection level.

- Unprotected  
- LocalHeritageProtection  
- RegionalHeritageProtection  
- NationalHeritageProtection  
- UnescoCandidate  
- UnescoWorldHeritageSite  
- TemporarilyProtected  

### PhysicalCondition
Current physical state of the landmark.

- Preserved  
- WellMaintained  
- RequiresRestoration  
- PartiallyDestroyed  
- SeverelyDamaged  
- Ruined  
- Lost  

### AccessibilityStatus
Public access rules.

- FreeAccess  
- PaidAccess  
- TemporarilyClosed  
- RestrictedAccess  
- Inaccessible  

---

## Entities (business description)

### Country
Represents a sovereign country.

- Identified by name and short code
- Can be retired (soft-deleted)
- Cannot be removed while it still has active regions or cities

---

### Region
Represents an administrative subdivision of a country.

- Always belongs to a country
- Can be retired (soft-deleted)
- Cannot be removed while it still has active cities

---

### City
Represents a city where landmarks exist.

- Belongs to a country
- May optionally belong to a region
- Can be retired (soft-deleted)
- Cannot be removed while it contains landmarks

Cities must be unique within a country + region context by name.

---

### LandmarkTag
Represents a reusable descriptive label.

Examples:
- Castle
- Historical Site
- Fortress
- Cultural Heritage

Tags:
- are global
- are reusable
- are hard-deleted
- must have unique names

---

### Landmark
Represents a historical landmark.

A landmark:
- belongs to exactly one city
- has one protection status
- has one physical condition
- has one accessibility status
- may have multiple tags

Landmarks are hard-deleted.

Historical dating:
- `FirstMentionYear` represents the earliest known historical mention
- exact dates are intentionally not required

---

## API behavior (business-level)

### General principles
- All create operations are **idempotent**
- Creating a resource that already exists returns the existing resource
- Updates always result in the final desired state
- Deletes follow explicit rules (soft vs hard)

### Soft delete rules
Soft delete applies to:
- Country
- Region
- City

Soft-deleted items:
- are hidden from normal reads
- cannot be used for creating new dependent entities
- are returned as "not found" by default reads

### Hard delete rules
Hard delete applies to:
- Landmark
- LandmarkTag

### Duplicate handling (idempotency)
- Creating a Country, Region, City, or LandmarkTag with the same identity does not fail
- The existing entity is returned instead
- Identity is defined by business uniqueness (normalized name + context)

---

## Business rules (explicit)

### Location integrity
- A City must belong to an existing, active Country
- If a City specifies a Region:
  - the Region must exist
  - the Region must belong to the same Country
  - the Region must not be deleted
- A Landmark must belong to an existing, active City

### Deletion rules
- A Country cannot be deleted while it has active Regions or Cities
- A Region cannot be deleted while it has active Cities
- A City cannot be deleted while it has Landmarks

### Visibility rules
- Landmarks belonging to deleted cities are not visible
- Deleted countries/regions/cities behave as non-existent in reads

---

## Out of scope (for now)
- Authentication and authorization
- Users, roles, permissions
- Payments or ticketing
- Moderation workflows
- Audit history
- File uploads (may be added later, but not required now)
- Background processing
