using HV.BLL.DTO.LandmarkTag;
using HV.DAL.Entities;

namespace HV.BLL.Mapping;

public static class LandmarkTagMappingExtensions
{
    extension(LandmarkTag tag)
    {
        public LandmarkTagListItemDto ToListItemDto()
        {
            return new LandmarkTagListItemDto(tag.Id, tag.Name, tag.Description);
        }

        public LandmarkTagDetailsDto ToDetailsDto()
        {
            return new LandmarkTagDetailsDto(tag.Id, tag.Name, tag.Description);
        }

        public void UpdateFrom(UpdateLandmarkTagRequest request, string normalizedName)
        {
            tag.Name = request.Name;
            tag.NormalizedName = normalizedName;
            tag.Description = request.Description;
        }
    }

    extension(CreateLandmarkTagRequest request)
    {
        public LandmarkTag ToEntity(string normalizedName)
        {
            return new LandmarkTag
            {
                Name = request.Name,
                NormalizedName = normalizedName,
                Description = request.Description
            };
        }
    }

    extension(IEnumerable<LandmarkTag> tags)
    {
        public List<LandmarkTagListItemDto> ToListItemDtoList()
        {
            return tags.Select(t => t.ToListItemDto()).ToList();
        }
    }
}

