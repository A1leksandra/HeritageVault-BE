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
    }

    extension(IEnumerable<Country> countries)
    {
        public IEnumerable<CountryListItemDto> ToListItemDtos()
        {
            return countries.Select(c => c.ToListItemDto());
        }

        public List<CountryListItemDto> ToListItemDtoList()
        {
            return countries.Select(c => c.ToListItemDto()).ToList();
        }
    }
}

