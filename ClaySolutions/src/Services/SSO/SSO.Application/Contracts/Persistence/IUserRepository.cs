using SharedKernel.Common;
using SSO.Application.Features.User.Commands.DeleteUser;
using SSO.Application.Features.User.Queries.GetUsers;
using SSO.Domain.Entities;
using System.Threading.Tasks;

namespace SSO.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<PaginatedResult<User>> GetAsync(string userName, string firstName, string lastName, bool? isActive, int pageNumber, int pageSize);
        Task<User> GetAsync(long id);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User> GetUserWithRolesAsync(string userName, string encPass);
        Task<User> GetUserWithRolesAsync(long id);
        Task<User> GetUserByPasswordAsync(long userId, string encPass);
        Task<User> GetWithRoleAndRefreshTokensAsync(long userId);
        Task<bool> IsUniqueUserNameAsync(string userName);
    }
}
