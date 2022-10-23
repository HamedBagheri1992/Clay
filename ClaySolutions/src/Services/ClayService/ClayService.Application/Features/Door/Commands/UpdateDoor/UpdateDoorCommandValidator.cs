using ClayService.Application.Contracts.Persistence;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Commands.UpdateDoor
{
    public class UpdateDoorCommandValidator : AbstractValidator<UpdateDoorCommand>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IOfficeRepository _officeRepository;
        public UpdateDoorCommandValidator(IDoorRepository doorRepository, IOfficeRepository officeRepository)
        {
            _doorRepository = doorRepository;
            _officeRepository = officeRepository;

            RuleFor(p => p.Name)
             .NotEmpty().WithMessage("{Name} is required.")
             .NotNull()
             .MaximumLength(200).WithMessage("{Name} must not exceed 200 characters.");


            RuleFor(p => p.Id)
                .GreaterThan(0)
                .WithMessage("{DoorId} is required.");

            RuleFor(p => p).MustAsync(BeUniqueName).WithMessage("The specified name already exists.");

            RuleFor(p => p.OfficeId)
                .GreaterThan(0)
                .WithMessage("{OfficeId} is required.")
                .MustAsync(BeOfficeIdInvalid).WithMessage("The specified officeId is invalid.");
        }

        public async Task<bool> BeUniqueName(UpdateDoorCommand command, CancellationToken cancellationToken)
        {
            return await _doorRepository.IsUniqueNameAsync(command.Id, command.Name, command.OfficeId);
        }

        public async Task<bool> BeOfficeIdInvalid(long officeId, CancellationToken cancellationToken)
        {
            return await _officeRepository.GetAsync(officeId) != null;
        }
    }
}
