using HV.DAL.Entities.Abstractions;

namespace HV.DAL.Entities;

public class LandmarkTag : BaseEntity
{
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
    public string? Description { get; set; }
}

