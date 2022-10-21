using FluentValidation;

namespace ClayService.Application.Features.Door.Commands.AssignDoor
{
    public class AssignDoorCommandValidator : AbstractValidator<AssignDoorCommand>
    {
        public AssignDoorCommandValidator()
        {
            RuleFor(p => p.UserId)
                .GreaterThan(0)
                .WithMessage("{UserId} is required.");

            RuleFor(p => p.DoorId)
                .GreaterThan(0)
                .WithMessage("{DoorId} is required.");
        }
    }
}
