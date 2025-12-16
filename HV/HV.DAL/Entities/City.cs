using HV.DAL.Entities.Abstractions;

namespace HV.DAL.Entities;

public class City : SoftDeletableEntity
{
    public required int CountryId { get; set; }
    public int? RegionId { get; set; }
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}

