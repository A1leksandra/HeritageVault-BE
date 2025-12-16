using HV.BLL.DTO.Region;

namespace HV.BLL.Services;

public interface IRegionService
{
    Task<IEnumerable<RegionListItemDto>> GetRegionsAsync(GetRegionsQuery query);
    Task<RegionDetailsDto> GetRegionByIdAsync(int id);
    Task<RegionDetailsDto> CreateRegionAsync(CreateRegionRequest request);
    Task<RegionDetailsDto> UpdateRegionAsync(int id, UpdateRegionRequest request);
    Task DeleteRegionAsync(int id);
}

