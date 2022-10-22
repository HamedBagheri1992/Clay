using AutoMapper;
using SharedKernel.Common;
using SSO.Application.Features.Role.Queries.GetRole;
using SSO.Application.Features.User.Commands.UpdateUser;
using SSO.Application.Features.User.Queries.GetUser;
using SSO.Domain.Entities;

namespace SSO.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedList<>));           
            
            CreateMap<Role, RoleDto>();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UpdateUserCommand, User>();
        }
    }
}
