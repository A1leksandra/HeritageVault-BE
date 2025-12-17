using HV.BLL.DTO.Region;

namespace HV.BLL.Services.Abstractions;

public interface IRegionService
{
    Task<IEnumerable<RegionListItemDto>> GetListAsync(GetRegionsQuery query);
    Task<RegionDetailsDto> GetByIdAsync(int id);
    Task<RegionDetailsDto> CreateAsync(CreateRegionRequest request);
    Task<RegionDetailsDto> UpdateAsync(int id, UpdateRegionRequest request);
    Task DeleteAsync(int id);
}

