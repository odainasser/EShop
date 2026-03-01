using Application.Features.Lookups;
using FluentValidation;

namespace Application.Validators.Lookups;

public class UpdateLookupRequestValidator : AbstractValidator<UpdateLookupRequest>
{
    public UpdateLookupRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("CodeRequired");

        RuleFor(x => x.NameEn)
            .NotEmpty().WithMessage("NameRequired");

        RuleFor(x => x.NameAr)
            .NotEmpty().WithMessage("NameRequired");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Required");
    }
}
