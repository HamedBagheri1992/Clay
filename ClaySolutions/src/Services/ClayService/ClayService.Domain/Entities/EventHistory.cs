using ClayService.Domain.Enums;
using System;

namespace ClayService.Domain.Entities
{
    public class EventHistory : EntityBase
    {
        public long UserId { get; set; }
        public string TagCode { get; set; }
        public long OfficeId { get; set; }
        public long DoorId { get; set; }
        public SourceType SourceType { get; set; }
        public bool OperationResult { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
