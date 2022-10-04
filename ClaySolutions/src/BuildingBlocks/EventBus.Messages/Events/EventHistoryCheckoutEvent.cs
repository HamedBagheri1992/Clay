using System;

namespace EventBus.Messages.Events
{
    public class EventHistoryCheckoutEvent
    {
        public long? UserId { get; set; }
        public string TagCode { get; set; }
        public long OfficeId { get; set; }
        public long DoorId { get; set; }
        public byte SourceType { get; set; }
        public bool OperationResult { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
