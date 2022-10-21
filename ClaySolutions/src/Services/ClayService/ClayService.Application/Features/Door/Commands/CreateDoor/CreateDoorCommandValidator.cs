using ClayService.Application.Contracts.Persistence;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Commands.CreateDoor
{
    public class CreateDoorCommandValidator : AbstractValidator<CreateDoorCommand>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IOfficeRepository _officeRepository;
        public CreateDoorCommandValidator(IDoorRepository doorRepository, IOfficeRepository officeRepository)
        {
            _doorRepository = doorRepository;
            _officeRepository = officeRepository;

            RuleFor(p => p.Name)
             .NotEmpty().WithMessage("{Name} is required.")
             .NotNull()
             .MaximumLength(200).WithMessage("{Name} must not exceed 200 characters.");

            RuleFor(p => p.OfficeId)
               .GreaterThan(0)
               .WithMessage("{OfficeId} is required.")
               .MustAsync(BeOfficeIdValid).WithMessage("The specified officeId is invalid.");

            RuleFor(p => p).MustAsync(BeUniqueName).WithMessage("The specified name already exists.");

        }

        public async Task<bool> BeUniqueName(CreateDoorCommand command, CancellationToken cancellationToken)
        {
            return await _doorRepository.IsUniqueNameAsync(command.Name, command.OfficeId);
        }

        public async Task<bool> BeOfficeIdValid(long officeId, CancellationToken cancellationToken)
        {
            return await _officeRepository.GetAsync(officeId) != null;
        }
    }
}
