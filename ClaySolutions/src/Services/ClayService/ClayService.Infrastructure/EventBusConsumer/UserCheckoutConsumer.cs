using ClayService.Application.Features.User.Commands.UserAddOrUpdate;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.EventBusConsumer
{
    public class UserCheckoutConsumer : IConsumer<UserCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserCheckoutConsumer> _logger;

        public UserCheckoutConsumer(IMediator mediator, ILogger<UserCheckoutConsumer> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserCheckoutEvent> context)
        {
            _logger.LogInformation($"User inforamtion Received => [{context.Message.Id}/{context.Message.UserName}");
            var command = new UserAddOrUpdateCommand { Id = context.Message.UserId, UserName = context.Message.UserName, DisplayName = context.Message.DisplayName };
            var result = await _mediator.Send(command);
            if (result)
            {
                _logger.LogInformation($"UsercheckoutEvent consumed successfully. AddedOrUpdated User => {command.UserName}");
                await Task.CompletedTask;
            }
            else
            {
                await Task.CompletedTask;
            }
        }
    }
}
