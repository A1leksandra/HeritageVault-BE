using Microsoft.AspNetCore.Http;

namespace HV.BLL.Services.Abstractions;

public interface IFileStorageService
{
    Task<(string RelativePath, string PublicUrl)> SaveLandmarkImageAsync(IFormFile file);
    Task DeleteFileIfExistsAsync(string? relativePath);
}

