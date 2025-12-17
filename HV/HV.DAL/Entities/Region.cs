using HV.DAL.Entities.Abstractions;

namespace HV.DAL.Entities;

public class Region : SoftDeletableEntity
{
    public required int CountryId { get; set; }
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
    public string? Type { get; set; }

    public Country Country { get; set; } = null!;
    public ICollection<City> Cities { get; set; } = new List<City>();
}

