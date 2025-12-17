namespace HV.BLL.DTO.City;

public record CityDetailsDto(int Id, int CountryId, string CountryName, int? RegionId, string? RegionName, string Name, decimal? Latitude, decimal? Longitude);

