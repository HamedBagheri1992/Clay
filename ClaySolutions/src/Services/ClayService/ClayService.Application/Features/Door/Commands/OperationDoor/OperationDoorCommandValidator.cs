using FluentValidation;

namespace ClayService.Application.Features.Door.Commands.OperationDoor
{
    public class OperationDoorCommandValidator : AbstractValidator<OperationDoorCommand>
    {
        public OperationDoorCommandValidator()
        {
            RuleFor(p => p.DoorId)
                 .GreaterThan(0)
                 .WithMessage("{DoorId} is required.");

            RuleFor(p => p.UserId)
                .GreaterThan(0)
                .WithMessage("{UserId} is required.");
        }
    }
}
