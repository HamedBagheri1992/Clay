using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Tag.Queries.GetTag;
using MediatR;
using SharedKernel.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Tag.Queries.MyTag
{
    public class MyTagQueryHandler : IRequestHandler<MyTagQuery, TagDto>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public MyTagQueryHandler(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<TagDto> Handle(MyTagQuery request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetTagOfUserAsync(request.UserId);
            if (tag == null)
                throw new BadRequestException($"User does not have PhysicalTag, {request.UserId}");

            return _mapper.Map<TagDto>(tag);
        }
    }
}
