using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Tag.Queries.GetTag;
using MediatR;
using SharedKernel.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Tag.Queries.GetTags
{
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, PaginatedList<TagDto>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public GetTagsQueryHandler(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _tagRepository.GetAsync(request);
            return _mapper.Map<PaginatedList<TagDto>>(tags);
        }
    }
}
