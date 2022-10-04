using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Domain.Entities;
using ClayService.Infrastructure.Persistence;
using EFCore.BulkExtensions;
using EventBus.Messages.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
                var histories = _mapper.Map<List<EventHistory>>(eventHistories);
                await _context.BulkInsertAsync(histories);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Bulk Insert");
                return false;
            }

        }
    }
}
