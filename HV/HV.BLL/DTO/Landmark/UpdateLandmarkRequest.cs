using HV.DAL.Enums;

namespace HV.BLL.DTO.Landmark;

public record UpdateLandmarkRequest(
    int CityId,
    string Name,
    string? Description,
    string? Address,
    decimal? Latitude,
    decimal? Longitude,
    int? FirstMentionYear,
    ProtectionStatus ProtectionStatus,
    PhysicalCondition PhysicalCondition,
    AccessibilityStatus AccessibilityStatus,
    string? ExternalRegistryUrl);

