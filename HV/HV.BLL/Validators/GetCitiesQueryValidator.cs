using FluentValidation;
using HV.BLL.DTO.City;

namespace HV.BLL.Validators;

public sealed class GetCitiesQueryValidator : AbstractValidator<GetCitiesQuery>
{
    public GetCitiesQueryValidator()
    {
    }
}

