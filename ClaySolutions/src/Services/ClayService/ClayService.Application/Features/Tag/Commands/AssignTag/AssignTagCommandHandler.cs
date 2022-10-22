using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Tag.Commands.AssignTag
{
    public class AssignTagCommandHandler : IRequestHandler<AssignTagCommand>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AssignTagCommandHandler> _logger;

        public AssignTagCommandHandler(ITagRepository tagRepository, IUserRepository userRepository, ICacheService cacheService, ILogger<AssignTagCommandHandler> logger)
        {
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Unit> Handle(AssignTagCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(Domain.Entities.User), request.UserId);

            var tag = await _tagRepository.GetAsync(request.TagId);
            if (tag == null)
                throw new NotFoundException(nameof(Domain.Entities.PhysicalTag), request.TagId);

            if (request.RemoveRequest == true)
                user.PhysicalTagId = null;
            else
                user.PhysicalTagId = tag.Id;

            await _userRepository.AssignTagAsync(user);

            if (request.RemoveRequest == true)
                _cacheService.DeleteTag(tag.TagCode);
            else
            {
                var result = _cacheService.AddOrUpdateTag(tag.TagCode, user.Id);
                if (result == false)
                    _logger.LogWarning($"Error on Cache server, UserId=>{user.Id}, TagCode=>{tag.TagCode}");
            }

            return Unit.Value;
        }
    }
}
