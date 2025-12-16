using HV.BLL.DTO.City;

namespace HV.BLL.Services;

public interface ICityService
{
    Task<IEnumerable<CityListItemDto>> GetCitiesAsync(GetCitiesQuery query);
    Task<CityDetailsDto> GetCityByIdAsync(int id);
    Task<CityDetailsDto> CreateCityAsync(CreateCityRequest request);
    Task<CityDetailsDto> UpdateCityAsync(int id, UpdateCityRequest request);
    Task DeleteCityAsync(int id);
}

