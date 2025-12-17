using HV.BLL.DTO.Country;

namespace HV.BLL.Services.Abstractions;

public interface ICountryService
{
    Task<IEnumerable<CountryListItemDto>> GetListAsync(GetCountriesQuery query);
    Task<CountryDetailsDto> GetByIdAsync(int id);
    Task<CountryDetailsDto> CreateAsync(CreateCountryRequest request);
    Task<CountryDetailsDto> UpdateAsync(int id, UpdateCountryRequest request);
    Task DeleteAsync(int id);
}

