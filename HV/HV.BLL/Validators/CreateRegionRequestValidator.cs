using FluentValidation;
using HV.BLL.DTO.Region;

namespace HV.BLL.Validators;

public sealed class CreateRegionRequestValidator : AbstractValidator<CreateRegionRequest>
{
    public CreateRegionRequestValidator()
    {
        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(160)
            .WithMessage("Name must not exceed 160 characters.");

        RuleFor(x => x.Type)
            .MaximumLength(40)
            .WithMessage("Type must not exceed 40 characters.")
            .When(x => x.Type is not null);
    }
}

