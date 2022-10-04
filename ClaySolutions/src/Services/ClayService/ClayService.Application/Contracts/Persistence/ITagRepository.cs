using ClayService.Application.Features.Tag.Commands.AssignTag;
using ClayService.Application.Features.Tag.Commands.CreateTag;
using ClayService.Application.Features.Tag.Queries.GetTag;
using ClayService.Application.Features.Tag.Queries.GetTags;
using ClayService.Application.Features.Tag.Queries.MyTag;
using ClayService.Domain.Entities;
using SharedKernel.Common;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface ITagRepository
    {
        Task AssignTagToUserAsync(AssignTagCommand request);
        Task<PhysicalTag> CreateAsync(CreateTagCommand request);
        Task<PhysicalTag> GetAsync(GetTagQuery request);
        Task<PaginatedResult<PhysicalTag>> GetAsync(GetTagsQuery request);
        Task<PhysicalTag> GetAsync(MyTagQuery request);
    }
}
