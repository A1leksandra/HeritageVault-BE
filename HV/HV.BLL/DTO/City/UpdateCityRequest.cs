namespace HV.BLL.DTO.City;

public record UpdateCityRequest(int CountryId, int? RegionId, string Name, decimal? Latitude, decimal? Longitude);

