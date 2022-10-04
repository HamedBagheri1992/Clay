using ClayService.Application.Features.Door.Commands.AssignDoor;
using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Door.Commands.OperationDoor;
using ClayService.Application.Features.Door.Commands.UpdateDoor;
using ClayService.Application.Features.Door.Queries.GetDoor;
using ClayService.Application.Features.Door.Queries.GetDoors;
using ClayService.Application.Features.Door.Queries.MyDoors;
using ClayService.Domain.Entities;
using SharedKernel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Persistence
{
    public interface IDoorRepository
    {
        Task AssignDoorToUserAsync(AssignDoorCommand request);
        Task<Door> CreateAsync(CreateDoorCommand request);
        Task<Door> GetAsync(GetDoorQuery request);
        Task<PaginatedResult<Door>> GetAsync(GetDoorsQuery request);
        Task<List<Door>> GetAsync(MyDoorsQuery request);
        Task OperationAsync(OperationDoorCommand request);
        Task UpdateAsync(UpdateDoorCommand request);
    }
}
