using MediatR;

namespace ClayService.Application.Features.Tag.Queries.GetTag
{
    public class GetTagQuery : IRequest<TagDto>
    {
        public long Id { get; set; }
    }
}
