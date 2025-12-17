using HV.BLL.DTO.Country;
using HV.BLL.Exceptions;
using HV.BLL.Exceptions.Abstractions;
using HV.BLL.Mapping;
using HV.BLL.Services.Abstractions;
using HV.DAL.Abstractions;
using HV.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HV.BLL.Services;

public sealed class CountryService(
    IRepository<Country> countryRepository,
    IRepository<Region> regionRepository,
    IRepository<City> cityRepository,
    IUnitOfWork unitOfWork) : ICountryService
{
    private readonly IRepository<Country> _countryRepository = countryRepository;
    private readonly IRepository<Region> _regionRepository = regionRepository;
    private readonly IRepository<City> _cityRepository = cityRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<CountryListItemDto>> GetListAsync(GetCountriesQuery query)
    {
        var countries = _countryRepository.AsQueryable();

        if (!query.IncludeDeleted)
            countries = countries.Where(c => !c.IsDeleted);

        var result = await countries.ToListAsync();
        return result.ToListItemDtos();
    }

    public async Task<CountryDetailsDto> GetByIdAsync(int id)
    {
        var country = await _countryRepository
            .Where(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Country with id {id} was not found.");

        return country.ToDetailsDto();
    }

    public async Task<CountryDetailsDto> CreateAsync(CreateCountryRequest request)
    {
        var normalizedName = NormalizeName(request.Name);
        var normalizedCode = NormalizeCode(request.Code);

        var existingCountry = await _countryRepository
            .Where(c => c.NormalizedName == normalizedName || c.Code == normalizedCode)
            .FirstOrDefaultAsync();

        if (existingCountry is not null)
            return existingCountry.ToDetailsDto();

        var country = request.ToEntity(normalizedName, normalizedCode);

        await _countryRepository.InsertAsync(country);
        await _unitOfWork.SaveChangesAsync();

        return country.ToDetailsDto();
    }

    public async Task<CountryDetailsDto> UpdateAsync(int id, UpdateCountryRequest request)
    {
        var country = await _countryRepository
            .Where(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Country with id {id} was not found.");

        var normalizedName = NormalizeName(request.Name);
        var normalizedCode = NormalizeCode(request.Code);

        var conflictingCountry = await _countryRepository
            .Where(c => c.Id != id && (c.NormalizedName == normalizedName || c.Code == normalizedCode))
            .FirstOrDefaultAsync();

        if (conflictingCountry is not null)
        {
            var conflictField = conflictingCountry.NormalizedName == normalizedName ? "name" : "code";
            throw new IncorrectParametersException($"Country with the same {conflictField} already exists.");
        }

        country.UpdateFrom(request, normalizedName, normalizedCode);

        _countryRepository.Update(country);
        await _unitOfWork.SaveChangesAsync();

        return country.ToDetailsDto();
    }

    public async Task DeleteAsync(int id)
    {
        var country = await _countryRepository
            .Where(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Country with id {id} was not found.");

        var hasActiveRegions = await _regionRepository
            .Where(r => r.CountryId == id && !r.IsDeleted)
            .AnyAsync();

        if (hasActiveRegions)
            throw new IncorrectParametersException("Cannot delete country because it has active regions.");

        var hasActiveCities = await _cityRepository
            .Where(c => c.CountryId == id && !c.IsDeleted)
            .AnyAsync();

        if (hasActiveCities)
            throw new IncorrectParametersException("Cannot delete country because it has active cities.");

        _countryRepository.SoftDelete(country);
        await _unitOfWork.SaveChangesAsync();
    }

    private static string NormalizeName(string name)
    {
        return string.Join(" ", name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToUpperInvariant();
    }

    private static string NormalizeCode(string code)
    {
        return code.Trim().ToUpperInvariant();
    }
}

