using ClayService.Application.Common.Settings;
using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IDistributedCache _redisCache;
        private readonly IOptionsMonitor<CacheSettingsConfigurationModel> _options;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IUserRepository userRepository, ITagRepository tagRepository, IDistributedCache redisCache, IOptionsMonitor<CacheSettingsConfigurationModel> options, ILogger<CacheService> logger)
        {
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _redisCache = redisCache;
            _options = options;
            _logger = logger;
        }

        public async Task InitAsync()
        {
            if (_options.CurrentValue.InitData)
            {
                _logger.LogInformation($"InitData has started to TagDb");

                int retry = 0;
                while (retry < 3)
                {
                    try
                    {
                        await Task.Delay(2000);
                        var tagData = _userRepository.GetUsersWithPhysicalTag();
                        var data = tagData.Select(u => new KeyValueModel<string, long> { Key = u.PhysicalTag.TagCode, Value = u.Id }).ToList();

                        foreach (KeyValueModel<string, long> item in data)
                        {
                            AddOrUpdateTag(item.Key, item.Value);
                        }
                        break;
                    }
                    catch (InvalidOperationException)
                    {
                        retry++;
                        continue;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error on Init of CacheService", ex);
                        break;
                    }
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
            var physicalTag = _tagRepository.GetWithUser(tagCode);
            if (physicalTag != null && physicalTag.User != null)
                return physicalTag.User.Id;

            return null;
        }

        #endregion
    }
}
