using HV.DAL.Enums;

namespace HV.BLL.DTO.Landmark;

public record GetLandmarksQuery(
    int? CityId = null,
    int? CountryId = null,
    int? RegionId = null,
    ProtectionStatus? ProtectionStatus = null,
    PhysicalCondition? PhysicalCondition = null,
    AccessibilityStatus? AccessibilityStatus = null,
    int[]? TagIds = null,
    string? NameContains = null);

