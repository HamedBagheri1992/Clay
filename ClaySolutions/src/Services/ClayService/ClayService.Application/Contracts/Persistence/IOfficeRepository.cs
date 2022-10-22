using ClayService.Application.Features.Office.Commands.DeleteOffice;
using ClayService.Application.Features.Office.Commands.UpdateOffice;
using ClayService.Application.Features.Office.Queries.GetOffice;
using ClayService.Application.Features.Office.Queries.GetOffices;
using ClayService.Application.Features.Office.Queries.MyOffices;
using ClayService.Domain.Entities;
using SharedKernel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface IOfficeRepository
    {
        Task<Office> CreateAsync(Office office);
        Task DeleteAsync(Office office);
        Task<Office> GetAsync(long officeId);
        Task<PaginatedResult<Office>> GetAsync(string title, int pageNumber, int pageSize);
        Task<List<Office>> GetOfficesOfUserAsync(long userId);
        Task UpdateAsync(Office office);
        Task<bool> IsUniqueTitleAsync(string title);
        Task<bool> IsUniqueTitleAsync(long id, string title);
        Task AssignOfficeToUserAsync(long officeId, User user);
        Task<bool> IsOfficeAssignedToUser(long officeId, long userId);
    }
}
