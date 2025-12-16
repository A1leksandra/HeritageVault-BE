using HV.DAL.Entities.Abstractions;

namespace HV.DAL.Entities;

public class Region : SoftDeletableEntity
{
    public required int CountryId { get; set; }
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
    public string? Type { get; set; }
}

