using FluentValidation;

namespace SSO.Application.Features.Authentication.Queries.Authenticate
{
    public class AuthenticateQueryValidator : AbstractValidator<AuthenticateQuery>
    {
        public AuthenticateQueryValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(p => p.Password)
               .NotEmpty().WithMessage("{Password} is required.")
               .NotNull()
               .MaximumLength(50).WithMessage("{Password} must not exceed 50 characters.");
        }
    }
}

