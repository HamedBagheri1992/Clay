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
        Task DeleteAsync(DeleteOfficeCommand request);
        Task<Office> GetAsync(long officeId);
        Task<Office> GetAsync(GetOfficeQuery request);
        Task<PaginatedResult<Office>> GetAsync(GetOfficesQuery request);
        Task<List<Office>> GetAsync(MyOfficesQuery request);
        Task UpdateAsync(UpdateOfficeCommand request);        
        Task<bool> IsUniqueTitleAsync(string title);
        Task<bool> IsUniqueTitleAsync(long id, string title);
        Task AssignOfficeToUserAsync(long officeId, User user);
        Task<bool> IsOfficeAssignedToUser(long officeId, long userId);
    }
}
