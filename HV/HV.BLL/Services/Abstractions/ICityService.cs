using HV.BLL.DTO.City;

namespace HV.BLL.Services.Abstractions;

public interface ICityService
{
    Task<IEnumerable<CityListItemDto>> GetListAsync(GetCitiesQuery query);
    Task<CityDetailsDto> GetByIdAsync(int id);
    Task<CityDetailsDto> CreateAsync(CreateCityRequest request);
    Task<CityDetailsDto> UpdateAsync(int id, UpdateCityRequest request);
    Task DeleteAsync(int id);
}

