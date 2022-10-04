using FluentValidation;

namespace ClayService.Application.Features.Office.Commands.CreateOffice
{
    public class CreateOfficeCommandValidator : AbstractValidator<CreateOfficeCommand>
    {
        public CreateOfficeCommandValidator()
        {
            RuleFor(p => p.Title)
              .NotEmpty().WithMessage("{Title} is required.")
              .NotNull()
              .MaximumLength(200).WithMessage("{Title} must not exceed 200 characters.");
        }
    }
}
