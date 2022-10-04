using FluentValidation;

namespace ClayService.Application.Features.Tag.Commands.AssignTag
{
    public class AssignTagCommandValidator : AbstractValidator<AssignTagCommand>
    {
        public AssignTagCommandValidator()
        {
            RuleFor(p => p.UserId)
               .GreaterThan(0)
               .WithMessage("{UserId} is required.");

            RuleFor(p => p.TagId)
               .GreaterThan(0)
               .WithMessage("{TagId} is required.");
        }
    }
}
