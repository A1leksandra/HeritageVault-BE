namespace HV.BLL.DTO.City;

public record CityDetailsDto(int Id, int CountryId, int? RegionId, string Name, decimal? Latitude, decimal? Longitude);

