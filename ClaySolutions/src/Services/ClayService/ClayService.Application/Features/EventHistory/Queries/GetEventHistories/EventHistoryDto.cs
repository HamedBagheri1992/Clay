using ClayService.Domain.Enums;
using System;

namespace ClayService.Application.Features.EventHistory.Queries.GetEventHistories
{
    public class EventHistoryDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string UserName { get; set; }
        public string TagCode { get; set; }
        public long OfficeId { get; set; }
        public string OfficeName { get; set; }
        public long DoorId { get; set; }
        public string DoorName { get; set; }
        public SourceType SourceType { get; set; }
        public bool OperationResult { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
