using HV.DAL.Entities.Abstractions;
using HV.DAL.Enums;

namespace HV.DAL.Entities;

public class Landmark : BaseEntity
{
    public required int CityId { get; set; }
    public required string Name { get; set; }
    public string? NormalizedName { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public int? FirstMentionYear { get; set; }
    public required ProtectionStatus ProtectionStatus { get; set; }
    public required PhysicalCondition PhysicalCondition { get; set; }
    public required AccessibilityStatus AccessibilityStatus { get; set; }
    public string? ExternalRegistryUrl { get; set; }
    public string? UploadedImagePath { get; set; }
    public string? ImageUrl { get; set; }

    public City City { get; set; } = null!;
}

