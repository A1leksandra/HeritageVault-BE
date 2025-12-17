using HV.DAL.Enums;

namespace HV.BLL.DTO.Landmark;

public record LandmarkListItemDto(
    int Id,
    int CityId,
    string CityName,
    string Name,
    ProtectionStatus ProtectionStatus,
    PhysicalCondition PhysicalCondition,
    AccessibilityStatus AccessibilityStatus,
    string? ImageUrl,
    IReadOnlyList<TagDto> Tags);

