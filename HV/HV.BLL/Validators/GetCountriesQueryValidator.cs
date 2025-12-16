using FluentValidation;
using HV.BLL.DTO.Country;

namespace HV.BLL.Validators;

public sealed class GetCountriesQueryValidator : AbstractValidator<GetCountriesQuery>
{
    public GetCountriesQueryValidator()
    {
    }
}

