using HV.BLL.DTO.LandmarkTag;

namespace HV.BLL.Services.Abstractions;

public interface ILandmarkTagService
{
    Task<IEnumerable<LandmarkTagListItemDto>> GetListAsync(GetLandmarkTagsQuery query);
    Task<LandmarkTagDetailsDto> GetByIdAsync(int id);
    Task<LandmarkTagDetailsDto> CreateAsync(CreateLandmarkTagRequest request);
    Task<LandmarkTagDetailsDto> UpdateAsync(int id, UpdateLandmarkTagRequest request);
    Task DeleteAsync(int id);
}

