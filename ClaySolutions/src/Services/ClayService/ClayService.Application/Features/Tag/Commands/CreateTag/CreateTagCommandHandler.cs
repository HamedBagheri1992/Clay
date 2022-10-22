using AutoMapper;
using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Tag.Queries.GetTag;
using ClayService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Tag.Commands.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, TagDto>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMapper _mapper;

        public CreateTagCommandHandler(ITagRepository tagRepository, IDateTimeService dateTimeService, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _dateTimeService = dateTimeService;
            _mapper = mapper;
        }

        public async Task<TagDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = new PhysicalTag { TagCode = request.TagCode, CreatedDate = _dateTimeService.Now };
            var result = await _tagRepository.CreateAsync(tag);
            return _mapper.Map<TagDto>(result);
        }
    }
}
