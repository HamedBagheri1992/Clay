using FluentValidation;

namespace ClayService.Application.Features.Door.Commands.UpdateDoor
{
    public class UpdateDoorCommandValidator : AbstractValidator<UpdateDoorCommand>
    {
        public UpdateDoorCommandValidator()
        {
            RuleFor(p => p.Name)
             .NotEmpty().WithMessage("{Name} is required.")
             .NotNull()
             .MaximumLength(200).WithMessage("{Name} must not exceed 200 characters.");


            RuleFor(p => p.Id)
                .GreaterThan(0)
                .WithMessage("{DoorId} is required.");

            RuleFor(p => p.OfficeId)
                .GreaterThan(0)
                .WithMessage("{OfficeId} is required.");
        }
    }
}
