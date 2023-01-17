using _.Application.Features.Products.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace _.Application.Validators.Features.Products.Commands.AddEdit
{
    public class AddEditProductCommandValidator : AbstractValidator<AddEditSpeedGovCommand>
    {
        public AddEditProductCommandValidator(IStringLocalizer<AddEditProductCommandValidator> localizer)
        {
            RuleFor(request => request.PlateNummber)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["PlateNumber is required!"]);
            RuleFor(request => request.OwnerId).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["OwnerId  is required!"]);
            RuleFor(request => request.PhoneNumber).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["PhoneNumber  is required!"]);
            RuleFor(request => request.CartypeId)
                .GreaterThan(0).WithMessage(x => localizer["CarType is required!"]);
        }
    }
}