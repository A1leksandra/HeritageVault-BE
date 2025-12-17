using HV.DAL.Enums;

namespace HV.BLL.DTO.Landmark;

public record LandmarkDetailsDto(
    int Id,
    int CityId,
    string CityName,
    int? RegionId,
    string? RegionName,
    int CountryId,
    string CountryName,
    string Name,
    string? Description,
    string? Address,
    decimal? Latitude,
    decimal? Longitude,
    int? FirstMentionYear,
    ProtectionStatus ProtectionStatus,
    PhysicalCondition PhysicalCondition,
    AccessibilityStatus AccessibilityStatus,
    string? ExternalRegistryUrl,
    string? UploadedImagePath,
    string? ImageUrl,
    IReadOnlyList<TagDto> Tags);

