using ClayService.Application.Features.Door.Queries.GetDoor;
using MediatR;

namespace ClayService.Application.Features.Door.Queries.MyDoors
{
    public class MyDoorsQuery : IRequest<DoorDto>
    {
        public long UserId { get; set; }

        public MyDoorsQuery(long userId)
        {
            UserId = userId;
        }
    }
}
