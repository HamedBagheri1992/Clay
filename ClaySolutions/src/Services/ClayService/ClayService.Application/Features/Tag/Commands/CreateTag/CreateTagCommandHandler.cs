﻿using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Tag.Queries.GetTag;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Tag.Commands.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, TagDto>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public CreateTagCommandHandler(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<TagDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.CreateAsync(request);
            return _mapper.Map<TagDto>(tag);
        }
    }
}