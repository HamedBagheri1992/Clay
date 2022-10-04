using FluentValidation;

namespace ClayService.Application.Features.Tag.Commands.CreateTag
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(p => p.TagCode)
                .NotEmpty()
                .NotNull()
                .WithMessage("{TagCode} is required.");
        }
    }
}
