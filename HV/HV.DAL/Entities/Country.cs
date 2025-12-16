using HV.DAL.Entities.Abstractions;

namespace HV.DAL.Entities;

public class Country : SoftDeletableEntity
{
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
    public required string Code { get; set; }
}

