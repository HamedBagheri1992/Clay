using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using ClayService.Domain.Enums;
using EventBus.Messages.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedKernel.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Door.Commands.OperationDoor
{
    public class OperationDoorCommandHandler : IRequestHandler<OperationDoorCommand>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeviceService _deviceService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<OperationDoorCommandHandler> _logger;

        public OperationDoorCommandHandler(IDoorRepository doorRepository, IUserRepository userRepository, IDeviceService deviceService, IDateTimeService dateTimeService, IKafkaProducer kafkaProducer, ILogger<OperationDoorCommandHandler> logger)
        {
            _doorRepository = doorRepository;
            _userRepository = userRepository;
            _deviceService = deviceService;
            _dateTimeService = dateTimeService;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task<Unit> Handle(OperationDoorCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserWithPhysicalTagAsync(request.UserId);
            if (user is null)
                throw new NotFoundException(nameof(Domain.Entities.User), request.UserId);

            if (user.PhysicalTag is null)
                throw new BadRequestException("You do not have PhysicalTag");

            var door = await _doorRepository.GetAsync(request.DoorId);
            if (door is null)
                throw new NotFoundException(nameof(Domain.Entities.Door), request.DoorId);

            if (await _doorRepository.IsDoorAssignedToUser(request.DoorId, user.Id) == false)
                throw new BadRequestException("Access denied to door");


            bool operationResult = await _deviceService.SendCommand(user.PhysicalTag.TagCode);

            //Log
            var eventHistoryCheckout = new EventHistoryCheckoutEvent()
            {
                UserId = request.UserId,
                TagCode = user.PhysicalTag.TagCode,
                OperationResult = operationResult,
                OfficeId = door.OfficeId,
                DoorId = door.Id,
                SourceType = (byte)SourceType.Appication,
                CreatedDate = _dateTimeService.Now
            };

            KafkaProduceMessage(eventHistoryCheckout);

            if (operationResult == false)
            {
                _logger.LogInformation($"The Opeartion failed for Door {door.Name} by User {user.UserName}");
                throw new ApiException("The Operation failed");
            }

            return Unit.Value;
        }

        private void KafkaProduceMessage(EventHistoryCheckoutEvent eventHistoryCheckout)
        {
            try
            {
                string message = JsonConvert.SerializeObject(eventHistoryCheckout);
                Task.Factory.StartNew(async () => await _kafkaProducer.WriteMessageAsync(message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "KafkaProducer has error");
            }
        }
    }
}
