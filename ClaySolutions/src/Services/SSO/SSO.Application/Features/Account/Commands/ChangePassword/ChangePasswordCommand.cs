using MediatR;

namespace SSO.Application.Features.Account.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest
    {
        public long UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
