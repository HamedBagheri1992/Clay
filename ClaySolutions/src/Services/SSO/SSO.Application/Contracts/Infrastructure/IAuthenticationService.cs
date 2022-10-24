using SSO.Application.Features.Authentication.Commands.ChangePassword;
using SSO.Application.Features.Authentication.Commands.UpdateUserRole;
using SSO.Application.Features.Authentication.Queries.Authenticate;
using SSO.Application.Features.Authentication.Queries.LogoutUser;
using SSO.Application.Features.Authentication.Queries.RefreshToken;
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
