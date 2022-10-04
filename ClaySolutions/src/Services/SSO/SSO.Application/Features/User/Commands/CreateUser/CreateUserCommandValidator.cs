using FluentValidation;

namespace SSO.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(p => p.UserName)
               .NotEmpty().WithMessage("{UserName} is required.")
               .NotNull()
               .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(p => p.Password)
               .NotEmpty().WithMessage("{Password} is required.")
               .NotNull()
               .MaximumLength(50).WithMessage("{Password} must not exceed 50 characters.");

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
