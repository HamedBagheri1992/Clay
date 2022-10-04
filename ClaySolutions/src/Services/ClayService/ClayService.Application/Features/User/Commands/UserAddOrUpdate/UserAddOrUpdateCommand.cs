using MediatR;

namespace ClayService.Application.Features.User.Commands.UserAddOrUpdate
{
    public class UserAddOrUpdateCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }
}
