using FluentValidation;
using HV.BLL.DTO.City;

namespace HV.BLL.Validators;

public sealed class UpdateCityRequestValidator : AbstractValidator<UpdateCityRequest>
{
    public UpdateCityRequestValidator()
    {
        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0.");

        RuleFor(x => x.RegionId)
            .GreaterThan(0)
            .WithMessage("RegionId must be greater than 0.")
            .When(x => x.RegionId is not null);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(160)
            .WithMessage("Name must not exceed 160 characters.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.")
            .When(x => x.Latitude is not null);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.")
            .When(x => x.Longitude is not null);
    }
}

