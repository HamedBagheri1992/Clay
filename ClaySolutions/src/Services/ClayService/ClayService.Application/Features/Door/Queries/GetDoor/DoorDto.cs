using System;

namespace ClayService.Application.Features.Door.Queries.GetDoor
{
    public class DoorDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long OfficeId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
