using HV.BLL.DTO.Region;
using HV.DAL.Entities;

namespace HV.BLL.Mapping;

public static class RegionMappingExtensions
{
    extension(Region region)
    {
        public RegionListItemDto ToListItemDto()
        {
            return new RegionListItemDto(region.Id, region.CountryId, region.Name, region.Type);
        }

        public RegionDetailsDto ToDetailsDto()
        {
            return new RegionDetailsDto(region.Id, region.CountryId, region.Country.Name, region.Name, region.Type);
        }

        public void UpdateFrom(UpdateRegionRequest request, string normalizedName)
        {
            region.CountryId = request.CountryId;
            region.Name = request.Name;
            region.NormalizedName = normalizedName;
            region.Type = request.Type;
        }
    }

    extension(CreateRegionRequest request)
    {
        public Region ToEntity(string normalizedName)
        {
            return new Region
            {
                CountryId = request.CountryId,
                Name = request.Name,
                NormalizedName = normalizedName,
                Type = request.Type,
                IsDeleted = false
            };
        }
    }

    extension(IEnumerable<Region> regions)
    {
        public IEnumerable<RegionListItemDto> ToListItemDtos()
        {
            return regions.Select(r => r.ToListItemDto());
        }
    }
}

