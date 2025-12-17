using FluentValidation;
using HV.BLL.DTO.LandmarkTag;

namespace HV.BLL.Validators;

public sealed class GetLandmarkTagsQueryValidator : AbstractValidator<GetLandmarkTagsQuery>
{
    public GetLandmarkTagsQueryValidator()
    {
    }
}

