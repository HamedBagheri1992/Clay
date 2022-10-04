using ClayService.Application.Features.Tag.Queries.GetTag;
using MediatR;

namespace ClayService.Application.Features.Tag.Queries.MyTag
{
    public class MyTagQuery : IRequest<TagDto>
    {
        public long UserId { get; set; }

        public MyTagQuery(long userId)
        {
            UserId = userId;
        }
    }
}
