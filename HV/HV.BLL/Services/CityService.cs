using HV.BLL.DTO.City;
using HV.BLL.Exceptions;
using HV.BLL.Exceptions.Abstractions;
using HV.BLL.Mapping;
using HV.DAL.Abstractions;
using HV.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HV.BLL.Services;

public sealed class CityService(
    IRepository<City> cityRepository,
    IRepository<Country> countryRepository,
    IRepository<Region> regionRepository,
    IUnitOfWork unitOfWork) : ICityService
{
    private readonly IRepository<City> _cityRepository = cityRepository;
    private readonly IRepository<Country> _countryRepository = countryRepository;
    private readonly IRepository<Region> _regionRepository = regionRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<CityListItemDto>> GetCitiesAsync(GetCitiesQuery query)
    {
        var cities = _cityRepository.AsQueryable();

        if (!query.IncludeDeleted)
            cities = cities.Where(c => !c.IsDeleted);

        if (query.CountryId is not null)
            cities = cities.Where(c => c.CountryId == query.CountryId.Value);

        if (query.RegionId is not null)
            cities = cities.Where(c => c.RegionId == query.RegionId.Value);

        if (query.NameContains is not null)
        {
            var normalizedSearch = NormalizeName(query.NameContains);
            cities = cities.Where(c => c.NormalizedName.Contains(normalizedSearch));
        }

        var result = await cities.ToListAsync();
        return result.ToListItemDtos();
    }

    public async Task<CityDetailsDto> GetCityByIdAsync(int id)
    {
        var city = await _cityRepository
            .Where(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"City with id {id} was not found.");

        return city.ToDetailsDto();
    }

    public async Task<CityDetailsDto> CreateCityAsync(CreateCityRequest request)
    {
        var countryExists = await _countryRepository
            .Where(c => c.Id == request.CountryId && !c.IsDeleted)
            .AnyAsync();

        if (!countryExists)
            throw new NotFoundException($"Country with id {request.CountryId} was not found.");

        if (request.RegionId is not null)
        {
            var regionExists = await _regionRepository
                .Where(r => r.Id == request.RegionId.Value && !r.IsDeleted)
                .AnyAsync();

            if (!regionExists)
                throw new NotFoundException($"Region with id {request.RegionId.Value} was not found.");

            var regionBelongsToCountry = await _regionRepository
                .Where(r => r.Id == request.RegionId.Value && r.CountryId == request.CountryId)
                .AnyAsync();

            if (!regionBelongsToCountry)
                throw new IncorrectParametersException("Region does not belong to the specified country.");
        }

        var normalizedName = NormalizeName(request.Name);

        var existingCity = await FindExistingCityAsync(request.CountryId, request.RegionId, normalizedName);

        if (existingCity is not null)
            return existingCity.ToDetailsDto();

        var city = request.ToEntity(normalizedName);

        await _cityRepository.InsertAsync(city);
        await _unitOfWork.SaveChangesAsync();

        return city.ToDetailsDto();
    }

    public async Task<CityDetailsDto> UpdateCityAsync(int id, UpdateCityRequest request)
    {
        var city = await _cityRepository
            .Where(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"City with id {id} was not found.");

        var countryExists = await _countryRepository
            .Where(c => c.Id == request.CountryId && !c.IsDeleted)
            .AnyAsync();

        if (!countryExists)
            throw new NotFoundException($"Country with id {request.CountryId} was not found.");

        if (request.RegionId is not null)
        {
            var regionExists = await _regionRepository
                .Where(r => r.Id == request.RegionId.Value && !r.IsDeleted)
                .AnyAsync();

            if (!regionExists)
                throw new NotFoundException($"Region with id {request.RegionId.Value} was not found.");

            var regionBelongsToCountry = await _regionRepository
                .Where(r => r.Id == request.RegionId.Value && r.CountryId == request.CountryId)
                .AnyAsync();

            if (!regionBelongsToCountry)
                throw new IncorrectParametersException("Region does not belong to the specified country.");
        }

        var normalizedName = NormalizeName(request.Name);

        var conflictingCity = await FindExistingCityAsync(request.CountryId, request.RegionId, normalizedName, excludeId: id);

        if (conflictingCity is not null)
            throw new IncorrectParametersException("City with the same name already exists in this country and region.");

        city.UpdateFrom(request, normalizedName);

        _cityRepository.Update(city);
        await _unitOfWork.SaveChangesAsync();

        return city.ToDetailsDto();
    }

    public async Task DeleteCityAsync(int id)
    {
        var city = await _cityRepository
            .Where(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"City with id {id} was not found.");

        // TODO: Check for Landmarks when they are implemented
        // If Landmark entity is not implemented yet, throw exception
        // When Landmark is implemented, check for NOT-deleted Landmarks and throw if any exist
        throw new IncorrectParametersException("Cannot delete city while Landmarks are not implemented.");

        _cityRepository.SoftDelete(city);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<City?> FindExistingCityAsync(int countryId, int? regionId, string normalizedName, int? excludeId = null)
    {
        var query = _cityRepository
            .Where(c => c.CountryId == countryId && c.NormalizedName == normalizedName);

        if (excludeId is not null)
            query = query.Where(c => c.Id != excludeId.Value);

        if (regionId is not null)
            query = query.Where(c => c.RegionId == regionId.Value);

        return await query.FirstOrDefaultAsync();
    }

    private static string NormalizeName(string name)
    {
        return string.Join(" ", name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToUpperInvariant();
    }
}

