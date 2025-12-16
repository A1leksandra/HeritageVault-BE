using HV.BLL.DTO.Region;
using HV.BLL.Exceptions;
using HV.BLL.Exceptions.Abstractions;
using HV.BLL.Mapping;
using HV.DAL.Abstractions;
using HV.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HV.BLL.Services;

public sealed class RegionService(
    IRepository<Region> regionRepository,
    IRepository<Country> countryRepository,
    IRepository<City> cityRepository,
    IUnitOfWork unitOfWork) : IRegionService
{
    private readonly IRepository<Region> _regionRepository = regionRepository;
    private readonly IRepository<Country> _countryRepository = countryRepository;
    private readonly IRepository<City> _cityRepository = cityRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<RegionListItemDto>> GetRegionsAsync(GetRegionsQuery query)
    {
        var regions = _regionRepository.AsQueryable();

        if (!query.IncludeDeleted)
            regions = regions.Where(r => !r.IsDeleted);

        if (query.CountryId is not null)
            regions = regions.Where(r => r.CountryId == query.CountryId.Value);

        var result = await regions.ToListAsync();
        return result.ToListItemDtos();
    }

    public async Task<RegionDetailsDto> GetRegionByIdAsync(int id)
    {
        var region = await _regionRepository
            .Where(r => r.Id == id && !r.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Region with id {id} was not found.");

        return region.ToDetailsDto();
    }

    public async Task<RegionDetailsDto> CreateRegionAsync(CreateRegionRequest request)
    {
        var countryExists = await _countryRepository
            .Where(c => c.Id == request.CountryId && !c.IsDeleted)
            .AnyAsync();

        if (!countryExists)
            throw new NotFoundException($"Country with id {request.CountryId} was not found.");

        var normalizedName = NormalizeName(request.Name);

        var existingRegion = await _regionRepository
            .Where(r => r.CountryId == request.CountryId && r.NormalizedName == normalizedName)
            .FirstOrDefaultAsync();

        if (existingRegion is not null)
            return existingRegion.ToDetailsDto();

        var region = request.ToEntity(normalizedName);

        await _regionRepository.InsertAsync(region);
        await _unitOfWork.SaveChangesAsync();

        return region.ToDetailsDto();
    }

    public async Task<RegionDetailsDto> UpdateRegionAsync(int id, UpdateRegionRequest request)
    {
        var region = await _regionRepository
            .Where(r => r.Id == id && !r.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Region with id {id} was not found.");

        var countryExists = await _countryRepository
            .Where(c => c.Id == request.CountryId && !c.IsDeleted)
            .AnyAsync();

        if (!countryExists)
            throw new NotFoundException($"Country with id {request.CountryId} was not found.");

        var normalizedName = NormalizeName(request.Name);

        var conflictingRegion = await _regionRepository
            .Where(r => r.Id != id && r.CountryId == request.CountryId && r.NormalizedName == normalizedName)
            .FirstOrDefaultAsync();

        if (conflictingRegion is not null)
            throw new IncorrectParametersException("Region with the same name already exists in this country.");

        region.UpdateFrom(request, normalizedName);

        _regionRepository.Update(region);
        await _unitOfWork.SaveChangesAsync();

        return region.ToDetailsDto();
    }

    public async Task DeleteRegionAsync(int id)
    {
        var region = await _regionRepository
            .Where(r => r.Id == id && !r.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Region with id {id} was not found.");

        var hasActiveCities = await _cityRepository
            .Where(c => c.RegionId == id && !c.IsDeleted)
            .AnyAsync();

        if (hasActiveCities)
            throw new IncorrectParametersException("Cannot delete region because it has active cities.");

        _regionRepository.SoftDelete(region);
        await _unitOfWork.SaveChangesAsync();
    }

    private static string NormalizeName(string name)
    {
        return string.Join(" ", name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToUpperInvariant();
    }
}

