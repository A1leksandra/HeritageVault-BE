using FluentValidation;
using HV.BLL.DTO.Country;

namespace HV.BLL.Validators;

public sealed class CreateCountryRequestValidator : AbstractValidator<CreateCountryRequest>
{
    public CreateCountryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(120)
            .WithMessage("Name must not exceed 120 characters.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required.")
            .MinimumLength(2)
            .WithMessage("Code must be at least 2 characters.")
            .MaximumLength(3)
            .WithMessage("Code must not exceed 3 characters.");
    }
}

