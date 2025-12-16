namespace HV.BLL.DTO.Region;

public record GetRegionsQuery(int? CountryId = null, bool IncludeDeleted = false);

