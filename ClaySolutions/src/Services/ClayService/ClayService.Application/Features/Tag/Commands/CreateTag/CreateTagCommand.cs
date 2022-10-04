using ClayService.Application.Features.Tag.Queries.GetTag;
using MediatR;
using System;

namespace ClayService.Application.Features.Tag.Commands.CreateTag
{
    public class CreateTagCommand : IRequest<TagDto>
    {
        public Guid TagCode { get; set; }
    }
}
