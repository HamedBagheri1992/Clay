using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Tag.Commands.AssignTag
{
    public class AssignTagCommandHandler : IRequestHandler<AssignTagCommand>
    {
        private readonly ITagRepository _tagRepository;

        public AssignTagCommandHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Unit> Handle(AssignTagCommand request, CancellationToken cancellationToken)
        {
            await _tagRepository.AssignTagToUserAsync(request);
            return Unit.Value;
        }
    }
}
