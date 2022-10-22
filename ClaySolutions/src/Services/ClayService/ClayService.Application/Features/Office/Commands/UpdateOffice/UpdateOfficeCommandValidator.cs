using ClayService.Application.Contracts.Persistence;
using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

namespace ClayService.Application.Features.Office.Commands.UpdateOffice
{
    public class UpdateOfficeCommandValidator : AbstractValidator<UpdateOfficeCommand>
    {
        private readonly IOfficeRepository _officeRepository;

        public UpdateOfficeCommandValidator(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;

            RuleFor(p => p.Title)
              .NotEmpty().WithMessage("{Title} is required.")
              .NotNull()
              .MaximumLength(200).WithMessage("{Title} must not exceed 200 characters.");

            RuleFor(p => p).MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");

            RuleFor(p => p.Id)
               .GreaterThan(0)
               .WithMessage("{OfficeId} is required.");
        }

        public async Task<bool> BeUniqueTitle(UpdateOfficeCommand command, CancellationToken arg2)
        {
            return await _officeRepository.IsUniqueTitleAsync(command.Id, command.Title);
        }
    }
}
