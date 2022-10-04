using FluentValidation;

namespace ClayService.Application.Features.EventHistory.Queries.GetEventHistories
{
    public class GetEventHistoriesQueryValidator : AbstractValidator<GetEventHistoriesQuery>
    {
        public GetEventHistoriesQueryValidator()
        {
            RuleFor(p => p.StartCreatedDate)
                .NotEmpty()
                .WithMessage("{StartCreatedDate} is required");

            RuleFor(p => p.EndCreatedDate)
                .NotEmpty()
                .WithMessage("{EndCreatedDate} is required");
        }
    }
}
