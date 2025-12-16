namespace HV.BLL.DTO.City;

public record CreateCityRequest(int CountryId, int? RegionId, string Name, decimal? Latitude, decimal? Longitude);

