using ClayService.Application.Features.Tag.Queries.GetTag;
using MediatR;
using SharedKernel.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Tag.Queries.GetTags
{
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, PaginatedList<TagDto>>
    {
        public Task<PaginatedList<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
