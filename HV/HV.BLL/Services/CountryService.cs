using HV.BLL.DTO.Country;
using HV.BLL.Exceptions;
using HV.BLL.Exceptions.Abstractions;
using HV.BLL.Mapping;
using HV.DAL.Abstractions;
using HV.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HV.BLL.Services;

public sealed class CountryService(
    IRepository<Country> countryRepository,
    IUnitOfWork unitOfWork) : ICountryService
{
    private readonly IRepository<Country> _countryRepository = countryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<CountryListItemDto>> GetCountriesAsync(GetCountriesQuery query)
    {
        var countries = _countryRepository.AsQueryable();

        if (!query.IncludeDeleted)
            countries = countries.Where(c => !c.IsDeleted);

        var result = await countries.ToListAsync();
        return result.ToListItemDtos();
    }

    public async Task<CountryDetailsDto> GetCountryByIdAsync(int id)
    {
        var country = await _countryRepository
            .Where(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Country with id {id} was not found.");

        return country.ToDetailsDto();
    }

    public async Task<CountryDetailsDto> CreateCountryAsync(CreateCountryRequest request)
    {
        var normalizedName = NormalizeName(request.Name);
        var normalizedCode = NormalizeCode(request.Code);

        var existingCountry = await _countryRepository
            .Where(c => c.NormalizedName == normalizedName || c.Code == normalizedCode)
            .FirstOrDefaultAsync();

        if (existingCountry is not null)
            return existingCountry.ToDetailsDto();

        var country = new Country
        {
            Name = request.Name,
            NormalizedName = normalizedName,
            Code = normalizedCode,
            IsDeleted = false
        };

        await _countryRepository.InsertAsync(country);
        await _unitOfWork.SaveChangesAsync();

        return country.ToDetailsDto();
    }

    public async Task<CountryDetailsDto> UpdateCountryAsync(int id, UpdateCountryRequest request)
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

        country.Name = request.Name;
        country.NormalizedName = normalizedName;
        country.Code = normalizedCode;

        _countryRepository.Update(country);
        await _unitOfWork.SaveChangesAsync();

        return country.ToDetailsDto();
    }

    public async Task DeleteCountryAsync(int id)
    {
        var country = await _countryRepository
            .Where(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Country with id {id} was not found.");

        // TODO: Check for Regions and Cities when they are implemented
        // If Region/City entities are not implemented yet, the delete restriction check will be added later
        // For now, we allow deletion. When Region/City are implemented, add checks:
        // - Cannot delete if there are any NOT-deleted Regions for this country
        // - Cannot delete if there are any NOT-deleted Cities for this country

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

