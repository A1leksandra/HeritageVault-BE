using HV.BLL.Exceptions.Abstractions;
using HV.BLL.Services.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HV.WebAPI.Services;

public sealed class FileStorageService(IWebHostEnvironment environment) : IFileStorageService
{
    private readonly IWebHostEnvironment _environment = environment;
    private const string LandmarkImagesFolder = "images/landmarks";
    private static readonly string[] AllowedExtensions = [".png", ".jpg", ".jpeg", ".webp", ".gif", ".bmp"];
    private static readonly string[] AllowedContentTypes = ["image/png", "image/jpeg", "image/jpg", "image/webp", "image/gif", "image/bmp"];

    public async Task<(string RelativePath, string PublicUrl)> SaveLandmarkImageAsync(IFormFile file)
    {
        if (file is null || file.Length == 0)
            throw new IncorrectParametersException("File is required and must not be empty.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
            throw new IncorrectParametersException($"File extension '{extension}' is not allowed. Allowed extensions: {string.Join(", ", AllowedExtensions)}");

        var contentType = file.ContentType.ToLowerInvariant();
        if (string.IsNullOrEmpty(contentType) || !AllowedContentTypes.Any(ct => string.Equals(contentType, ct, StringComparison.OrdinalIgnoreCase)))
            throw new IncorrectParametersException($"Content type '{contentType}' is not allowed. Allowed content types: {string.Join(", ", AllowedContentTypes)}");

        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrEmpty(webRootPath))
        {
            var contentRootPath = _environment.ContentRootPath;
            webRootPath = Path.Combine(contentRootPath, "wwwroot");
            if (!Directory.Exists(webRootPath))
                Directory.CreateDirectory(webRootPath);
        }

        var imagesFolder = Path.Combine(webRootPath, LandmarkImagesFolder);
        if (!Directory.Exists(imagesFolder))
            Directory.CreateDirectory(imagesFolder);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var relativePath = $"/{LandmarkImagesFolder}/{fileName}";
        var fullPath = Path.Combine(webRootPath, LandmarkImagesFolder, fileName);

        await using var fileStream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(fileStream);

        var publicUrl = relativePath;

        return (relativePath, publicUrl);
    }

    public Task DeleteFileIfExistsAsync(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return Task.CompletedTask;

        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrEmpty(webRootPath))
        {
            var contentRootPath = _environment.ContentRootPath;
            webRootPath = Path.Combine(contentRootPath, "wwwroot");
        }

        if (string.IsNullOrEmpty(webRootPath))
            return Task.CompletedTask;

        var fullPath = Path.Combine(webRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch
            {
                // Ignore deletion errors
            }
        }

        return Task.CompletedTask;
    }
}

