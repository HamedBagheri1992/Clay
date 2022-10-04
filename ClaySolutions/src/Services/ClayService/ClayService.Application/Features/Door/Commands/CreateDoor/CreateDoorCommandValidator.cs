using FluentValidation;

namespace ClayService.Application.Features.Door.Commands.CreateDoor
{
    public class CreateDoorCommandValidator : AbstractValidator<CreateDoorCommand>
    {
        public CreateDoorCommandValidator()
        {
            RuleFor(p => p.Name)
              .NotEmpty().WithMessage("{Name} is required.")
              .NotNull()
              .MaximumLength(200).WithMessage("{Name} must not exceed 200 characters.");


            RuleFor(p => p.OfficeId)
                .GreaterThan(0)
                .WithMessage("{OfficeId} is required.");
        }
    }
}
