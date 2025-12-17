using HV.BLL.DTO.LandmarkTag;
using HV.BLL.Exceptions;
using HV.BLL.Exceptions.Abstractions;
using HV.BLL.Helpers;
using HV.BLL.Mapping;
using HV.BLL.Services.Abstractions;
using HV.DAL.Abstractions;
using HV.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HV.BLL.Services;

public sealed class LandmarkTagService(
    IRepository<LandmarkTag> tagRepository,
    IUnitOfWork unitOfWork) : ILandmarkTagService
{
    private readonly IRepository<LandmarkTag> _tagRepository = tagRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<LandmarkTagListItemDto>> GetListAsync(GetLandmarkTagsQuery query)
    {
        var tags = _tagRepository.AsQueryable();

        if (query.NameContains is not null)
        {
            var normalizedSearch = NameNormalizer.Normalize(query.NameContains);
            tags = tags.Where(t => t.NormalizedName.Contains(normalizedSearch));
        }

        var result = await tags.ToListAsync();
        return result.ToListItemDtoList();
    }

    public async Task<LandmarkTagDetailsDto> GetByIdAsync(int id)
    {
        var tag = await _tagRepository
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"LandmarkTag with id {id} was not found.");

        return tag.ToDetailsDto();
    }

    public async Task<LandmarkTagDetailsDto> CreateAsync(CreateLandmarkTagRequest request)
    {
        var normalizedName = NameNormalizer.Normalize(request.Name);

        var existingTag = await _tagRepository
            .Where(t => t.NormalizedName == normalizedName)
            .FirstOrDefaultAsync();

        if (existingTag is not null)
            return existingTag.ToDetailsDto();

        var tag = request.ToEntity(normalizedName);

        await _tagRepository.InsertAsync(tag);
        await _unitOfWork.SaveChangesAsync();

        return tag.ToDetailsDto();
    }

    public async Task<LandmarkTagDetailsDto> UpdateAsync(int id, UpdateLandmarkTagRequest request)
    {
        var tag = await _tagRepository
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"LandmarkTag with id {id} was not found.");

        var normalizedName = NameNormalizer.Normalize(request.Name);

        var conflictingTag = await _tagRepository
            .Where(t => t.Id != id && t.NormalizedName == normalizedName)
            .FirstOrDefaultAsync();

        if (conflictingTag is not null)
            throw new IncorrectParametersException("LandmarkTag with the same name already exists.");

        tag.UpdateFrom(request, normalizedName);

        _tagRepository.Update(tag);
        await _unitOfWork.SaveChangesAsync();

        return tag.ToDetailsDto();
    }

    public async Task DeleteAsync(int id)
    {
        var tag = await _tagRepository
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"LandmarkTag with id {id} was not found.");

        _tagRepository.Delete(tag);
        await _unitOfWork.SaveChangesAsync();
    }
}

