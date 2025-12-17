using FluentValidation;
using HV.BLL.DTO.LandmarkTag;

namespace HV.BLL.Validators;

public sealed class CreateLandmarkTagRequestValidator : AbstractValidator<CreateLandmarkTagRequest>
{
    public CreateLandmarkTagRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(80)
            .WithMessage("Name must not exceed 80 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(400)
            .WithMessage("Description must not exceed 400 characters.")
            .When(x => x.Description is not null);
    }
}

