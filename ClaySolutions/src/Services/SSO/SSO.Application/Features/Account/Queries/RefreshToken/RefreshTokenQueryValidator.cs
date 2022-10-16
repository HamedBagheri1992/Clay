using FluentValidation;

namespace SSO.Application.Features.Account.Queries.RefreshToken
{
    public class RefreshTokenQueryValidator : AbstractValidator<RefreshTokenQuery>
    {
        public RefreshTokenQueryValidator()
        {
            RuleFor(p => p.AccessToken)
                .NotEmpty().WithMessage("{AccessToken} is required.")
                .NotNull();

            RuleFor(p => p.RefreshToken)
               .NotEmpty().WithMessage("{RefreshToken} is required.")
               .NotNull();
        }
    }
}
