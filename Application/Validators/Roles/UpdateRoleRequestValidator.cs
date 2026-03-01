using Application.Features.Roles;
using FluentValidation;

namespace Application.Validators.Roles;

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator()
    {
        RuleFor(x => x.NameEn)
            .NotEmpty().WithMessage("English Role name is required.")
            .MaximumLength(256).WithMessage("English Role name must not exceed 256 characters.")
            .Matches(@"^[a-zA-Z\s]*$").WithMessage("English Role name can only contain letters and spaces.");

        RuleFor(x => x.NameAr)
            .NotEmpty().WithMessage("Arabic Role name is required.")
            .MaximumLength(256).WithMessage("Arabic Role name must not exceed 256 characters.");

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(500).WithMessage("English Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.DescriptionEn));

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(500).WithMessage("Arabic Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.DescriptionAr));
    }
}
