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

            RuleFor(p => p.DoorIds)
               .NotEmpty()
               .WithMessage("{DoorIds} is required.");
        }
    }
}
