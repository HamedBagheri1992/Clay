using ClayService.Application.Features.Tag.Queries.GetTag;
using MediatR;
using SharedKernel.Common;
using System;

namespace ClayService.Application.Features.Tag.Queries.GetTags
{
    public class GetTagsQuery : PaginationQuery, IRequest<PaginatedList<TagDto>>
    {
        public string TagCode { get; set; }
        public DateTime? StartCreatedDate { get; set; }
        public DateTime? EndCreatedDate { get; set; }
    }
}
