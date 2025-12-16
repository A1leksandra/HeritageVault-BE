using HV.BLL.DTO.Country;
using HV.DAL.Entities;

namespace HV.BLL.Mapping;

public static class CountryMappingExtensions
{
    extension(Country country)
    {
        public CountryListItemDto ToListItemDto()
        {
            return new CountryListItemDto(country.Id, country.Name, country.Code);
        }

        public CountryDetailsDto ToDetailsDto()
        {
            return new CountryDetailsDto(country.Id, country.Name, country.Code);
        }

        public void UpdateFrom(UpdateCountryRequest request, string normalizedName, string normalizedCode)
        {
            country.Name = request.Name;
            country.NormalizedName = normalizedName;
            country.Code = normalizedCode;
        }
    }

    extension(CreateCountryRequest request)
    {
        public Country ToEntity(string normalizedName, string normalizedCode)
        {
            return new Country
            {
                Name = request.Name,
                NormalizedName = normalizedName,
                Code = normalizedCode,
                IsDeleted = false
            };
        }
    }

    extension(IEnumerable<Country> countries)
    {
        public IEnumerable<CountryListItemDto> ToListItemDtos()
        {
            return countries.Select(c => c.ToListItemDto());
        }
    }
}

