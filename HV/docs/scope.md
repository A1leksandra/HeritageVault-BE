# HeritageVault — Project Scope

## Domain summary
HeritageVault is a public registry system for historical landmarks.  
The system stores and exposes information about landmarks, their locations, their historical condition, legal protection, and accessibility.

The platform is designed as an open, read-only friendly registry:
- no authentication
- no ownership
- no user-specific behavior

Primary use cases:
- maintain a clean, structured registry of countries, regions, cities, and landmarks
- browse and filter landmarks by location, condition, protection, and accessibility
- safely retire (soft-delete) obsolete reference data without losing historical context
- optionally attach a representative image to a landmark for presentation purposes

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

### Landmark image
A landmark may optionally have an image used for visual presentation in the frontend.

- The image may be uploaded to the system or referenced via a link.
- If an uploaded image already exists, uploading a new image replaces the previous one.
- An image may also be removed independently of the landmark itself.
- Image handling is optional and does not affect core landmark data or business rules.

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

### Landmark
Represents a historical landmark.

A landmark:
- belongs to exactly one city
- has one protection status
- has one physical condition
- has one accessibility status
- may optionally have an associated image

Landmarks are hard-deleted.

Historical dating:
- `FirstMentionYear` represents the earliest known historical mention
- exact dates are intentionally not required

Image notes:
- An uploaded image is stored by the system and associated with the landmark.
- A link suitable for frontend display is provided when an image exists.
- Image presence does not affect landmark identity or uniqueness.

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

### Duplicate handling (idempotency)
- Creating a Country, Region, or City with the same identity does not fail
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

### Image rules
- Uploading an image for a landmark replaces any previously uploaded image
- Deleting an image removes only the image, not the landmark
- Image operations do not affect other landmark data

---

## Out of scope (for now)
- Authentication and authorization
- Users, roles, permissions
- Payments or ticketing
- Moderation workflows
- Audit history
- Background processing
