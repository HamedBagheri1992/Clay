using FluentValidation;

namespace SSO.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(p => p.FirstName)
              .NotEmpty().WithMessage("{FirstName} is required.")
              .NotNull()
              .MaximumLength(100).WithMessage("{FirstName} must not exceed 100 characters.");

            RuleFor(p => p.LastName)
             .NotEmpty().WithMessage("{LastName} is required.")
             .NotNull()
             .MaximumLength(100).WithMessage("{LastName} must not exceed 100 characters.");

            RuleFor(p => p.IsActive)
             .NotEmpty().WithMessage("{IsActive} is required.").NotNull();
        }
    }
}
