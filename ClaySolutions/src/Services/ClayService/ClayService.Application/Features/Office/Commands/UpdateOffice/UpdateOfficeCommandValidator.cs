using FluentValidation;

namespace ClayService.Application.Features.Office.Commands.UpdateOffice
{
    public class UpdateOfficeCommandValidator : AbstractValidator<UpdateOfficeCommand>
    {
        public UpdateOfficeCommandValidator()
        {
            RuleFor(p => p.Title)
              .NotEmpty().WithMessage("{Title} is required.")
              .NotNull()
              .MaximumLength(200).WithMessage("{Title} must not exceed 200 characters.");

            RuleFor(p => p.Id)
               .GreaterThan(0)
               .WithMessage("{OfficeId} is required.");
        }
    }
}
