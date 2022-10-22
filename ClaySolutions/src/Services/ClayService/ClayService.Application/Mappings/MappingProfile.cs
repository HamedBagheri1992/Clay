using AutoMapper;
using ClayService.Application.Features.Door.Commands.UpdateDoor;
using ClayService.Application.Features.Door.Queries.GetDoor;
using ClayService.Application.Features.EventHistory.Queries.GetEventHistories;
using ClayService.Application.Features.Office.Commands.UpdateOffice;
using ClayService.Application.Features.Office.Queries.GetOffice;
using ClayService.Application.Features.Tag.Queries.GetTag;
using ClayService.Application.Features.User.Commands.UserAddOrUpdate;
using ClayService.Domain.Entities;
using SharedKernel.Common;

namespace ClayService.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedList<>));

            CreateMap<Office, OfficeDto>();
            CreateMap<UpdateOfficeCommand, Office>();

            CreateMap<Door, DoorDto>();
            CreateMap<UpdateDoorCommand, Door>();

            CreateMap<PhysicalTag, TagDto>();
            CreateMap<EventHistory, EventHistoryDto>();
            CreateMap<UserAddOrUpdateCommand, User>();
        }
    }
}
