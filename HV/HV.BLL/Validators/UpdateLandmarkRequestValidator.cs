using FluentValidation;
using HV.BLL.DTO.Landmark;

namespace HV.BLL.Validators;

public sealed class UpdateLandmarkRequestValidator : AbstractValidator<UpdateLandmarkRequest>
{
    public UpdateLandmarkRequestValidator()
    {
        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithMessage("CityId must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(5000)
            .WithMessage("Description must not exceed 5000 characters.")
            .When(x => x.Description is not null);

        RuleFor(x => x.Address)
            .MaximumLength(300)
            .WithMessage("Address must not exceed 300 characters.")
            .When(x => x.Address is not null);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.")
            .When(x => x.Latitude is not null);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.")
            .When(x => x.Longitude is not null);

        RuleFor(x => x.FirstMentionYear)
            .InclusiveBetween(1, 2100)
            .WithMessage("FirstMentionYear must be between 1 and 2100.")
            .When(x => x.FirstMentionYear is not null);

        RuleFor(x => x.ExternalRegistryUrl)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("ExternalRegistryUrl must be a valid absolute URL.")
            .When(x => x.ExternalRegistryUrl is not null)
            .MaximumLength(500)
            .WithMessage("ExternalRegistryUrl must not exceed 500 characters.")
            .When(x => x.ExternalRegistryUrl is not null);

        RuleFor(x => x.TagIds)
            .Must(tagIds => tagIds is null || tagIds.Length <= 50)
            .WithMessage("TagIds must not exceed 50 items.")
            .Must(tagIds => tagIds is null || tagIds.Distinct().Count() == tagIds.Length)
            .WithMessage("TagIds must contain unique values.")
            .When(x => x.TagIds is not null);
    }
}

