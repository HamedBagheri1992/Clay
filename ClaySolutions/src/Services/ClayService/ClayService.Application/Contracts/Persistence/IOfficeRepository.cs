using ClayService.Application.Features.Office.Commands.CreateOffice;
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
        Task<Office> CreateAsync(CreateOfficeCommand request);
        Task DeleteAsync(DeleteOfficeCommand request);
        Task<Office> GetAsync(GetOfficeQuery request);
        Task<PaginatedResult<Office>> GetAsync(GetOfficesQuery request);
        Task<List<Office>> GetAsync(MyOfficesQuery request);
        Task UpdateAsync(UpdateOfficeCommand request);
    }
}
