using ClayService.Application.Features.Door.Queries.GetDoor;
using MediatR;
using System.Collections.Generic;

namespace ClayService.Application.Features.Door.Queries.MyDoors
{
    public class MyDoorsQuery : IRequest<List<DoorDto>>
    {
        public long UserId { get; set; }

        public MyDoorsQuery(long userId)
        {
            UserId = userId;
        }
    }
}
