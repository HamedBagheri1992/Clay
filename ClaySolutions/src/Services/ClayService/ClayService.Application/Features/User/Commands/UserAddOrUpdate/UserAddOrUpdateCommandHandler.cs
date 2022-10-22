using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.User.Commands.UserAddOrUpdate
{
    public class UserAddOrUpdateCommandHandler : IRequestHandler<UserAddOrUpdateCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserAddOrUpdateCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UserAddOrUpdateCommandHandler(IUserRepository userRepository, ILogger<UserAddOrUpdateCommandHandler> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UserAddOrUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _mapper.Map<Domain.Entities.User>(request);
                await _userRepository.AddOrUpdateAsync(user);
                _logger.LogInformation($"User added to ClayService Database, UserName {user.UserName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on AddOrUpdate User");
                return false;
            }
        }
    }
}
