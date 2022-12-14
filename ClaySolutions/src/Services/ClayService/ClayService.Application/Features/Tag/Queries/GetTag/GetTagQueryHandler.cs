using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using MediatR;
using SharedKernel.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Tag.Queries.GetTag
{
    public class GetTagQueryHandler : IRequestHandler<GetTagQuery, TagDto>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public GetTagQueryHandler(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<TagDto> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetAsync(request.Id);
            if (tag == null)
                throw new NotFoundException(nameof(Domain.Entities.PhysicalTag), request.Id);

            return _mapper.Map<TagDto>(tag);
        }
    }
}
