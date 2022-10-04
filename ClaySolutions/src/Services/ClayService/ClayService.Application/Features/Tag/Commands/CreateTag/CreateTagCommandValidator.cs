using FluentValidation;
using System;

namespace ClayService.Application.Features.Tag.Commands.CreateTag
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(p => p.TagCode)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("{TagCode} is required.");
        }
    }
}
