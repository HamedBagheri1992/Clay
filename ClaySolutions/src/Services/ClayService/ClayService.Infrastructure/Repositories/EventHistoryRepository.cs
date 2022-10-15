﻿using AutoMapper;
using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.EventHistory.Queries.GetEventHistories;
using ClayService.Domain.Entities;
using ClayService.Domain.Enums;
using ClayService.Infrastructure.Persistence;
using EFCore.BulkExtensions;
using EventBus.Messages.Events;
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
        private readonly ICacheService _cacheService;

        public EventHistoryRepository(IMapper mapper, ClayServiceDbContext context, ILogger<EventHistoryRepository> logger, ICacheService cacheService)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<bool> BulkInsert(List<EventHistoryCheckoutEvent> eventHistories)
        {
            try
            {
                List<EventHistory> histories = MapHistories(eventHistories, _cacheService);
                await _context.BulkInsertAsync(histories);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Bulk Insert");
                return false;
            }

        }

        public async Task<PaginatedResult<EventHistoryDto>> GetAsync(GetEventHistoriesQuery request)
        {
            var query = from EH in _context.Set<EventHistory>()
                        from D in _context.Set<Door>().Where(D => D.Id == EH.DoorId)
                        from O in _context.Set<Office>().Where(O => O.Id == D.OfficeId)
                        from U in _context.Set<User>().Where(U => U.Id == EH.UserId)
                        select new EventHistoryDto
                        {
                            Id = EH.Id,
                            TagCode = EH.TagCode,
                            CreatedDate = EH.CreatedDate,
                            SourceType = EH.SourceType,
                            OperationResult = EH.OperationResult,
                            DoorId = EH.DoorId,
                            DoorName = D.Name,
                            OfficeId = EH.OfficeId,
                            OfficeName = O.Title,
                            UserId = U.Id,
                            UserDisplayName = U.DisplayName,
                            UserName = U.UserName
                        };

            query = query.Where(q => q.CreatedDate >= request.StartCreatedDate && q.CreatedDate <= request.EndCreatedDate);

            if (request.UserId.HasValue == true)
                query = query.Where(e => e.UserId == request.UserId.Value);

            if (request.DoorId.HasValue == true)
                query = query.Where(e => e.DoorId == request.DoorId.Value);

            return await query.ToPagedListAsync(request.PageNumber, request.PageSize);
        }


        #region Privates 

        private static List<EventHistory> MapHistories(List<EventHistoryCheckoutEvent> eventHistories, ICacheService cacheService)
        {
            return eventHistories.Select(item => new EventHistory
            {
                UserId = cacheService.GetUserId(item.TagCode),
                TagCode = item.TagCode,
                SourceType = (SourceType)item.SourceType,
                OfficeId = item.OfficeId,
                DoorId = item.DoorId,
                OperationResult = item.OperationResult,
                CreatedDate = item.CreatedDate
            }).ToList();
        }

        #endregion
    }
}
