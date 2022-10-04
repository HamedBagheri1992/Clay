using AutoMapper;
using ClayService.Application.Features.Door.Queries.GetDoor;
using ClayService.Application.Features.Office.Queries.GetOffice;
using ClayService.Application.Features.Tag.Queries.GetTag;
using ClayService.Application.Features.User.Commands.UserAddOrUpdate;
using ClayService.Domain.Entities;
using EventBus.Messages.Events;
using SharedKernel.Common;
using System.Collections.Generic;

namespace ClayService.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Office, OfficeDto>();
            CreateMap<PaginatedResult<Office>, PaginatedList<OfficeDto>>();

            CreateMap<Door, DoorDto>();
            CreateMap<PaginatedResult<Door>, PaginatedList<DoorDto>>();

            CreateMap<PhysicalTag, TagDto>();
            CreateMap<PaginatedResult<PhysicalTag>, PaginatedList<TagDto>>();

            CreateMap<UserAddOrUpdateCommand, User>();

            CreateMap<EventHistoryCheckoutEvent, EventHistory>();
        }
    }
}
