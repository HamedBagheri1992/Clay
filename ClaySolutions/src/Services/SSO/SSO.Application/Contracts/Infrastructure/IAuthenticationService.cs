using SSO.Application.Features.Account.Commands.ChangePassword;
using SSO.Application.Features.Account.Commands.UpdateUserRole;
using SSO.Application.Features.Account.Queries.Authenticate;
using SSO.Application.Features.Account.Queries.LogoutUser;
using SSO.Application.Features.Account.Queries.RefreshToken;
using System.Threading.Tasks;

namespace SSO.Application.Contracts.Infrastructure
{
    public interface IAuthenticationService
    {
        Task<AuthenticateDto> AuthenticateAsync(AuthenticateQuery request);
        Task ChangePasswordAsync(ChangePasswordCommand request);
        Task LogoutAsync(LogoutUserQuery request);
        Task<AuthenticateDto> RefreshTokenAsync(RefreshTokenQuery request);
        Task UpdateUserRoleAsync(UpdateUserRoleCommand request);
    }
}
