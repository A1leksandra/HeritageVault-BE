using HV.BLL.DTO.Landmark;
using HV.DAL.Entities;

namespace HV.BLL.Mapping;

public static class LandmarkMappingExtensions
{
    extension(Landmark landmark)
    {
        public LandmarkListItemDto ToListItemDto()
        {
            return new LandmarkListItemDto(
                landmark.Id,
                landmark.CityId,
                landmark.City.Name,
                landmark.Name,
                landmark.ProtectionStatus,
                landmark.PhysicalCondition,
                landmark.AccessibilityStatus,
                landmark.ImageUrl);
        }

        public LandmarkDetailsDto ToDetailsDto()
        {
            return new LandmarkDetailsDto(
                landmark.Id,
                landmark.CityId,
                landmark.City.Name,
                landmark.City.RegionId,
                landmark.City.Region?.Name,
                landmark.City.CountryId,
                landmark.City.Country.Name,
                landmark.Name,
                landmark.Description,
                landmark.Address,
                landmark.Latitude,
                landmark.Longitude,
                landmark.FirstMentionYear,
                landmark.ProtectionStatus,
                landmark.PhysicalCondition,
                landmark.AccessibilityStatus,
                landmark.ExternalRegistryUrl,
                landmark.UploadedImagePath,
                landmark.ImageUrl);
        }

        public void UpdateFrom(UpdateLandmarkRequest request, string? normalizedName)
        {
            landmark.CityId = request.CityId;
            landmark.Name = request.Name;
            landmark.NormalizedName = normalizedName;
            landmark.Description = request.Description;
            landmark.Address = request.Address;
            landmark.Latitude = request.Latitude;
            landmark.Longitude = request.Longitude;
            landmark.FirstMentionYear = request.FirstMentionYear;
            landmark.ProtectionStatus = request.ProtectionStatus;
            landmark.PhysicalCondition = request.PhysicalCondition;
            landmark.AccessibilityStatus = request.AccessibilityStatus;
            landmark.ExternalRegistryUrl = request.ExternalRegistryUrl;
        }
    }

    extension(CreateLandmarkRequest request)
    {
        public Landmark ToEntity(string? normalizedName)
        {
            return new Landmark
            {
                CityId = request.CityId,
                Name = request.Name,
                NormalizedName = normalizedName,
                Description = request.Description,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                FirstMentionYear = request.FirstMentionYear,
                ProtectionStatus = request.ProtectionStatus,
                PhysicalCondition = request.PhysicalCondition,
                AccessibilityStatus = request.AccessibilityStatus,
                ExternalRegistryUrl = request.ExternalRegistryUrl
            };
        }
    }

    extension(IEnumerable<Landmark> landmarks)
    {
        public List<LandmarkListItemDto> ToListItemDtoList()
        {
            return landmarks.Select(l => l.ToListItemDto()).ToList();
        }
    }
}

