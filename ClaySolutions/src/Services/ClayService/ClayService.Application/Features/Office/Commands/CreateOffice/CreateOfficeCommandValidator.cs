using ClayService.Application.Contracts.Persistence;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Office.Commands.CreateOffice
{
    public class CreateOfficeCommandValidator : AbstractValidator<CreateOfficeCommand>
    {
        private readonly IOfficeRepository _officeRepository;
        public CreateOfficeCommandValidator(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;

            RuleFor(p => p.Title)
              .NotEmpty().WithMessage("{Title} is required.")
              .NotNull()
              .MaximumLength(200).WithMessage("{Title} must not exceed 200 characters.")
              .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");

        }

        public async Task<bool> BeUniqueTitle(string title, CancellationToken arg2)
        {
            return await _officeRepository.IsUniqueTitleAsync(title);
        }
    }
}
