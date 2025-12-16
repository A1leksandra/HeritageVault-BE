using FluentValidation;
using HV.BLL.DTO.Region;

namespace HV.BLL.Validators;

public sealed class GetRegionsQueryValidator : AbstractValidator<GetRegionsQuery>
{
    public GetRegionsQueryValidator()
    {
    }
}

