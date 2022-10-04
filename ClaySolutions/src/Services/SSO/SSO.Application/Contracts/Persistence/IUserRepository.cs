using SharedKernel.Common;
using SSO.Application.Features.User.Commands.CreateUser;
using SSO.Application.Features.User.Commands.DeleteUser;
using SSO.Application.Features.User.Commands.UpdateUser;
using SSO.Application.Features.User.Queries.GetUser;
using SSO.Application.Features.User.Queries.GetUsers;
using SSO.Domain.Entities;
using System.Threading.Tasks;

namespace SSO.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<PaginatedResult<User>> GetAsync(GetUsersQuery request);
        Task<User> GetAsync(GetUserQuery request);
        Task<User> CreateAsync(CreateUserCommand request);
        Task UpdateAsync(UpdateUserCommand request);
        Task DeleteAsync(DeleteUserCommand request);        
    }
}
