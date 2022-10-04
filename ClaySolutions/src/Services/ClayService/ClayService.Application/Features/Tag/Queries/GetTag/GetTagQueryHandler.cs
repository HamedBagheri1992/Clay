using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Tag.Queries.GetTag
{
    public class GetTagQueryHandler : IRequestHandler<GetTagQuery, TagDto>
    {
        public Task<TagDto> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
