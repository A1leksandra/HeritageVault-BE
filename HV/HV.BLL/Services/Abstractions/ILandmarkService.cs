using HV.BLL.DTO.Landmark;

namespace HV.BLL.Services.Abstractions;

public interface ILandmarkService
{
    Task<IEnumerable<LandmarkListItemDto>> GetListAsync(GetLandmarksQuery query);
    Task<LandmarkDetailsDto> GetByIdAsync(int id);
    Task<LandmarkDetailsDto> CreateAsync(CreateLandmarkRequest request);
    Task<LandmarkDetailsDto> UpdateAsync(int id, UpdateLandmarkRequest request);
    Task DeleteAsync(int id);
}

