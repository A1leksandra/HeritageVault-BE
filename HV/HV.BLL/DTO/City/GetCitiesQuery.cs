namespace HV.BLL.DTO.City;

public record GetCitiesQuery(int? CountryId = null, int? RegionId = null, bool IncludeDeleted = false, string? NameContains = null);

