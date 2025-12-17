using FluentValidation;
using HV.BLL.DTO.Landmark;

namespace HV.BLL.Validators;

public sealed class GetLandmarksQueryValidator : AbstractValidator<GetLandmarksQuery>
{
    public GetLandmarksQueryValidator()
    {
    }
}

