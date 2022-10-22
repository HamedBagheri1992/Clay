using FluentValidation;
using System.Threading.Tasks;
using System.Threading;
using SSO.Application.Contracts.Persistence;

namespace SSO.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(p => p.UserName)
              .NotEmpty().WithMessage("{UserName} is required.")
              .NotNull()
              .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.")
              .MustAsync(BeUniqueUserName).WithMessage("The specified userName already exists.");

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

        public async Task<bool> BeUniqueUserName(string userName, CancellationToken arg2)
        {
            return await _userRepository.IsUniqueUserNameAsync(userName);
        }
    }
}
