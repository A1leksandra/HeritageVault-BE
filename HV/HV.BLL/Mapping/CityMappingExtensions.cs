using HV.BLL.DTO.City;
using HV.DAL.Entities;

namespace HV.BLL.Mapping;

public static class CityMappingExtensions
{
    extension(City city)
    {
        public CityListItemDto ToListItemDto()
        {
            return new CityListItemDto(city.Id, city.CountryId, city.RegionId, city.Name);
        }

        public CityDetailsDto ToDetailsDto()
        {
            return new CityDetailsDto(city.Id, city.CountryId, city.RegionId, city.Name, city.Latitude, city.Longitude);
        }

        public void UpdateFrom(UpdateCityRequest request, string normalizedName)
        {
            city.CountryId = request.CountryId;
            city.RegionId = request.RegionId;
            city.Name = request.Name;
            city.NormalizedName = normalizedName;
            city.Latitude = request.Latitude;
            city.Longitude = request.Longitude;
        }
    }

    extension(CreateCityRequest request)
    {
        public City ToEntity(string normalizedName)
        {
            return new City
            {
                CountryId = request.CountryId,
                RegionId = request.RegionId,
                Name = request.Name,
                NormalizedName = normalizedName,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                IsDeleted = false
            };
        }
    }

    extension(IEnumerable<City> cities)
    {
        public IEnumerable<CityListItemDto> ToListItemDtos()
        {
            return cities.Select(c => c.ToListItemDto());
        }
    }
}

