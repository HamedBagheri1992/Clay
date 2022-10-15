﻿using ClayService.Application.Common.Settings;
using ClayService.Application.Contracts.Infrastructure;
using ClayService.Domain.Entities;
using ClayService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly ClayServiceDbContext _context;
        private readonly IDistributedCache _redisCache;
        private readonly IOptionsMonitor<CacheSettingsConfigurationModel> _options;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IServiceProvider provider, IDistributedCache redisCache, IOptionsMonitor<CacheSettingsConfigurationModel> options, ILogger<CacheService> logger)
        {
            _context = provider.CreateScope().ServiceProvider.GetRequiredService<ClayServiceDbContext>();
            _redisCache = redisCache;
            _options = options;
            _logger = logger;
        }

        public async Task InitAsync()
        {
            if (_options.CurrentValue.InitData)
            {
                _logger.LogInformation($"InitData has started to TagDb");

                var tagData = await _context.Users.Include(u => u.PhysicalTag).AsNoTracking().Where(u => u.PhysicalTagId.HasValue == true)
                    .Select(u => new KeyValueModel<string, long> { Key = u.PhysicalTag.TagCode, Value = u.Id }).ToListAsync();

                foreach (KeyValueModel<string, long> item in tagData)
                {
                    AddOrUpdateTag(item.Key, item.Value);
                }
            }
        }

        public long GetUserId(string tagCode)
        {
            var redisResult = GetUserIdRedis(tagCode);
            if (redisResult != null)
            {
                _logger.LogInformation($"Get UserId From redis => {redisResult.Value} / {tagCode}");
                return redisResult.Value;
            }

            var contextResult = GetUserIdContext(tagCode);
            if (contextResult != null)
            {
                _logger.LogInformation($"Get UserId From Database => {contextResult.Value} / {tagCode}");

                AddOrUpdateTag(tagCode, redisResult.Value);
                return contextResult.Value;
            }

            _logger.LogWarning($"There is no UserId for tagCode => {tagCode}");
            return -1;
        }

        public bool AddOrUpdateTag(string tagCode, long userId)
        {
            try
            {
                _redisCache.SetString(tagCode, userId.ToString());
                _logger.LogInformation($"UserId added to TagDb => {userId} / {tagCode}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on set in Redis", ex);
                return false;
            }
        }

        public void DeleteTag(string tagCode)
        {
            _redisCache.Remove(tagCode);
            _logger.LogInformation($"TagCode deleted from TagDb => {tagCode}");
        }


        #region Privates

        private long? GetUserIdRedis(string tagCode)
        {
            var userId = _redisCache.GetString(tagCode);
            if (long.TryParse(userId, out long id))
                return id;

            return null;
        }

        private long? GetUserIdContext(string tagCode)
        {
            var user = _context.Users.Include(u => u.PhysicalTag).AsNoTracking().FirstOrDefault(u => u.PhysicalTag.TagCode == tagCode);
            if (user != null)
                return user.Id;

            return null;
        }

        #endregion
    }
}