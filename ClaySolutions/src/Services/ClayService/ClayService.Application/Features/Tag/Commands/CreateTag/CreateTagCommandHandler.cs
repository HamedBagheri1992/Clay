using ClayService.Application.Features.Tag.Queries.GetTag;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Tag.Commands.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, TagDto>
    {
        public Task<TagDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
