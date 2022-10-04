using MediatR;
using SharedKernel.Common;
using SSO.Application.Features.User.Queries.GetUser;

namespace SSO.Application.Features.User.Queries.GetUsers
{
    public class GetUsersQuery : PaginationQuery, IRequest<PaginatedList<UserDto>>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
    }
}
