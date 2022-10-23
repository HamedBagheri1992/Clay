using ClayService.Domain.Entities;
using SharedKernel.Common;
using System;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface ITagRepository
    {
        Task<PhysicalTag> CreateAsync(PhysicalTag tag);
        Task<PhysicalTag> GetAsync(long id);
        PhysicalTag GetWithUser(string tagCode);
        Task<PaginatedResult<PhysicalTag>> GetAsync(DateTime? startCreatedDate, DateTime? endCreatedDate, string tagCode, int pageNumber, int pageSize);
        Task<PhysicalTag> GetTagOfUserAsync(long userId);
        Task<bool> IsUniqueTagCodeAsync(string tagCode);
    }
}
