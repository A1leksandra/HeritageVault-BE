using HV.BLL.DTO.Country;

namespace HV.BLL.Services;

public interface ICountryService
{
    Task<IEnumerable<CountryListItemDto>> GetCountriesAsync(GetCountriesQuery query);
    Task<CountryDetailsDto> GetCountryByIdAsync(int id);
    Task<CountryDetailsDto> CreateCountryAsync(CreateCountryRequest request);
    Task<CountryDetailsDto> UpdateCountryAsync(int id, UpdateCountryRequest request);
    Task DeleteCountryAsync(int id);
}

