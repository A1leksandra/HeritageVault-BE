using HV.BLL.DTO.Landmark;
using Microsoft.AspNetCore.Http;

namespace HV.BLL.Services.Abstractions;

public interface ILandmarkService
{
    Task<IEnumerable<LandmarkListItemDto>> GetListAsync(GetLandmarksQuery query);
    Task<LandmarkDetailsDto> GetByIdAsync(int id);
    Task<LandmarkDetailsDto> CreateAsync(CreateLandmarkRequest request);
    Task<LandmarkDetailsDto> UpdateAsync(int id, UpdateLandmarkRequest request);
    Task DeleteAsync(int id);
    Task UploadImageAsync(int id, IFormFile file);
    Task DeleteImageAsync(int id);
}

