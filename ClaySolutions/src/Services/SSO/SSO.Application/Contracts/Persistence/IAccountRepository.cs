using SSO.Application.Features.Account.Commands.ChangePassword;
using SSO.Application.Features.Account.Commands.UpdateUserRole;
using SSO.Application.Features.Account.Queries.Authenticate;
using System.Threading.Tasks;

namespace SSO.Application.Contracts.Persistence
{
    public interface IAccountRepository
    {
        Task<AuthenticateDto> AuthenticateAsync(AuthenticateQuery request);
        Task ChangePasswordAsync(ChangePasswordCommand request);
        Task UpdateUserRoleAsync(UpdateUserRoleCommand request);
    }
}
