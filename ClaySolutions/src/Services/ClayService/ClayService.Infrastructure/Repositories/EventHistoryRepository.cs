using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.EventHistory.Queries.GetEventHistories;
using ClayService.Domain.Entities;
using ClayService.Domain.Enums;
using ClayService.Infrastructure.Persistence;
using EFCore.BulkExtensions;
using EventBus.Messages.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Common;
using SharedKernel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Repositories
{
    public class EventHistoryRepository : IEventHistoryRepository
    {
        private readonly IMapper _mapper;
        private readonly ClayServiceDbContext _context;
        private readonly ILogger<EventHistoryRepository> _logger;

        public EventHistoryRepository(IMapper mapper, ClayServiceDbContext context, ILogger<EventHistoryRepository> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> BulkInsert(List<EventHistoryCheckoutEvent> eventHistories)
        {
            try
            {
                List<EventHistory> histories = MapHistories(eventHistories);
                await _context.BulkInsertAsync(histories);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Bulk Insert");
                return false;
            }

        }

        private static List<EventHistory> MapHistories(List<EventHistoryCheckoutEvent> eventHistories)
        {
            return eventHistories.Select(item => new EventHistory
            {
                UserId = item.UserId,
                TagCode = item.TagCode,
                SourceType = (SourceType)item.SourceType,
                OfficeId = item.OfficeId,
                DoorId = item.DoorId,
                OperationResult = item.OperationResult,
                CreatedDate = item.CreatedDate
            }).ToList();
        }

        public async Task<PaginatedResult<EventHistory>> GetAsync(GetEventHistoriesQuery request)
        {
            var query = _context.EventHistories.AsNoTracking().Where(e => e.CreatedDate >= request.StartCreatedDate && e.CreatedDate <= request.EndCreatedDate);

            if (request.UserId.HasValue == true)
                query = query.Where(e => e.UserId.HasValue == true && e.UserId.Value == request.UserId.Value);

            if (request.DoorId.HasValue == true)
                query = query.Where(e => e.DoorId == request.DoorId.Value);

            return await query.ToPagedListAsync(request.PageNumber, request.PageSize);

        }
    }
}
