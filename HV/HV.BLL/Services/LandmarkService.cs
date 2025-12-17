using HV.BLL.DTO.Landmark;
using HV.BLL.Exceptions;
using HV.BLL.Exceptions.Abstractions;
using HV.BLL.Helpers;
using HV.BLL.Mapping;
using HV.BLL.Services.Abstractions;
using HV.DAL.Abstractions;
using HV.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HV.BLL.Services;

public sealed class LandmarkService(
    IRepository<Landmark> landmarkRepository,
    IRepository<LandmarkTag> tagRepository,
    IRepository<City> cityRepository,
    IFileStorageService fileStorageService,
    IUnitOfWork unitOfWork) : ILandmarkService
{
    private readonly IRepository<Landmark> _landmarkRepository = landmarkRepository;
    private readonly IRepository<LandmarkTag> _tagRepository = tagRepository;
    private readonly IRepository<City> _cityRepository = cityRepository;
    private readonly IFileStorageService _fileStorageService = fileStorageService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<LandmarkListItemDto>> GetListAsync(GetLandmarksQuery query)
    {
        var landmarks = _landmarkRepository.AsQueryable()
            .Where(l => !l.City.IsDeleted);

        if (query.CityId is not null)
            landmarks = landmarks.Where(l => l.CityId == query.CityId.Value);

        if (query.CountryId is not null)
            landmarks = landmarks.Where(l => l.City.CountryId == query.CountryId.Value);

        if (query.RegionId is not null)
            landmarks = landmarks.Where(l => l.City.RegionId == query.RegionId.Value);

        if (query.ProtectionStatus is not null)
            landmarks = landmarks.Where(l => l.ProtectionStatus == query.ProtectionStatus.Value);

        if (query.PhysicalCondition is not null)
            landmarks = landmarks.Where(l => l.PhysicalCondition == query.PhysicalCondition.Value);

        if (query.AccessibilityStatus is not null)
            landmarks = landmarks.Where(l => l.AccessibilityStatus == query.AccessibilityStatus.Value);

        if (query.TagIds?.Length > 0)
        {
            var tagIds = query.TagIds.Distinct().ToArray();
            landmarks = landmarks.Where(l => l.Tags.Any(t => tagIds.Contains(t.Id)));
        }

        if (query.NameContains is not null)
        {
            var normalizedSearch = NameNormalizer.Normalize(query.NameContains);
            landmarks = landmarks.Where(l => l.NormalizedName != null && l.NormalizedName.Contains(normalizedSearch));
        }

        var result = await landmarks
            .Include(l => l.Tags)
            .Include(l => l.City)
                .ThenInclude(c => c.Country)
            .Include(l => l.City)
                .ThenInclude(c => c.Region)
            .ToListAsync();

        return result.ToListItemDtoList();
    }

    public async Task<LandmarkDetailsDto> GetByIdAsync(int id)
    {
        var landmark = await _landmarkRepository
            .Include(l => l.Tags)
            .Include(l => l.City)
                .ThenInclude(c => c.Country)
            .Include(l => l.City)
                .ThenInclude(c => c.Region)
            .Where(l => l.Id == id && !l.City.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Landmark with id {id} was not found.");

        return landmark.ToDetailsDto();
    }

    public async Task<LandmarkDetailsDto> CreateAsync(CreateLandmarkRequest request)
    {
        var cityExists = await _cityRepository
            .Where(c => c.Id == request.CityId && !c.IsDeleted)
            .AnyAsync();

        if (!cityExists)
            throw new NotFoundException($"City with id {request.CityId} was not found.");

        var normalizedName = NameNormalizer.Normalize(request.Name);

        var existingLandmark = await _landmarkRepository
            .Include(l => l.Tags)
            .Include(l => l.City)
                .ThenInclude(c => c.Country)
            .Include(l => l.City)
                .ThenInclude(c => c.Region)
            .Where(l => l.CityId == request.CityId && l.NormalizedName == normalizedName)
            .FirstOrDefaultAsync();

        if (existingLandmark is not null)
            return existingLandmark.ToDetailsDto();

        ICollection<LandmarkTag> tags = new List<LandmarkTag>();

        if (request.TagIds is not null && request.TagIds.Length > 0)
        {
            var tagIds = request.TagIds.Distinct().ToArray();
            var foundTags = await _tagRepository
                .Where(t => tagIds.Contains(t.Id))
                .ToListAsync();

            if (foundTags.Count != tagIds.Length)
                throw new IncorrectParametersException("One or more tag IDs do not exist.");

            tags = foundTags;
        }

        var landmark = request.ToEntity(normalizedName, tags);

        await _landmarkRepository.InsertAsync(landmark);
        await _unitOfWork.SaveChangesAsync();

        var createdLandmark = await _landmarkRepository
            .Include(l => l.Tags)
            .Include(l => l.City)
                .ThenInclude(c => c.Country)
            .Include(l => l.City)
                .ThenInclude(c => c.Region)
            .Where(l => l.Id == landmark.Id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Landmark with id {landmark.Id} was not found.");

        return createdLandmark.ToDetailsDto();
    }

    public async Task<LandmarkDetailsDto> UpdateAsync(int id, UpdateLandmarkRequest request)
    {
        var landmark = await _landmarkRepository
            .Include(l => l.Tags)
            .Where(l => l.Id == id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Landmark with id {id} was not found.");

        var cityExists = await _cityRepository
            .Where(c => c.Id == request.CityId && !c.IsDeleted)
            .AnyAsync();

        if (!cityExists)
            throw new NotFoundException($"City with id {request.CityId} was not found.");

        ICollection<LandmarkTag> tags = new List<LandmarkTag>();

        if (request.TagIds is not null && request.TagIds.Length > 0)
        {
            var tagIds = request.TagIds.Distinct().ToArray();
            var foundTags = await _tagRepository
                .Where(t => tagIds.Contains(t.Id))
                .ToListAsync();

            if (foundTags.Count != tagIds.Length)
                throw new IncorrectParametersException("One or more tag IDs do not exist.");

            tags = foundTags;
        }

        var normalizedName = NameNormalizer.Normalize(request.Name);

        landmark.UpdateFrom(request, normalizedName, tags);

        _landmarkRepository.Update(landmark);
        await _unitOfWork.SaveChangesAsync();

        var updatedLandmark = await _landmarkRepository
            .Include(l => l.Tags)
            .Include(l => l.City)
                .ThenInclude(c => c.Country)
            .Include(l => l.City)
                .ThenInclude(c => c.Region)
            .Where(l => l.Id == id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Landmark with id {id} was not found.");

        return updatedLandmark.ToDetailsDto();
    }

    public async Task DeleteAsync(int id)
    {
        var landmark = await _landmarkRepository
            .Where(l => l.Id == id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Landmark with id {id} was not found.");

        _landmarkRepository.Delete(landmark);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UploadImageAsync(int id, IFormFile file)
    {
        var landmark = await _landmarkRepository
            .FirstOrDefaultAsync(l => l.Id == id) ?? throw new NotFoundException($"Landmark with id {id} was not found.");

        if (!string.IsNullOrEmpty(landmark.UploadedImagePath))
        {
            await _fileStorageService.DeleteFileIfExistsAsync(landmark.UploadedImagePath);
        }

        var (relativePath, publicUrl) = await _fileStorageService.SaveLandmarkImageAsync(file);

        landmark.UploadedImagePath = relativePath;
        landmark.ImageUrl = publicUrl;

        _landmarkRepository.Update(landmark);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteImageAsync(int id)
    {
        var landmark = await _landmarkRepository
            .FirstOrDefaultAsync(l => l.Id == id) ?? throw new NotFoundException($"Landmark with id {id} was not found.");

        if (!string.IsNullOrEmpty(landmark.UploadedImagePath))
        {
            await _fileStorageService.DeleteFileIfExistsAsync(landmark.UploadedImagePath);
        }

        landmark.UploadedImagePath = null;
        landmark.ImageUrl = null;

        _landmarkRepository.Update(landmark);
        await _unitOfWork.SaveChangesAsync();
    }
}

