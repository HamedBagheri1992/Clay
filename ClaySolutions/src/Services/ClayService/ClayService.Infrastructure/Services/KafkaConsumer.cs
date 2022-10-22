using ClayService.Application.Common.Settings;
using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using ClayService.Domain.Entities;
using ClayService.Domain.Enums;
using Confluent.Kafka;
using EventBus.Messages.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Services
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly IEventHistoryRepository _eventHistoryRepository;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IOptionsMonitor<KafkaSettingsConfigurationModel> _options;
        private IConsumer<Null, string> _consumer;
        private System.Timers.Timer _timer;
        private readonly ConcurrentQueue<ConsumeResult<Null, string>> messagesQueue = new ConcurrentQueue<ConsumeResult<Null, string>>();
        private readonly CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancelToken;
        private readonly ICacheService _cacheService;

        public KafkaConsumer(IEventHistoryRepository eventHistoryRepository, ICacheService cacheService, IOptionsMonitor<KafkaSettingsConfigurationModel> options, ILogger<KafkaConsumer> logger)
        {
            _eventHistoryRepository = eventHistoryRepository;
            _cacheService = cacheService;
            _options = options;
            _logger = logger;

            _cancellationTokenSource = new CancellationTokenSource();
        }

        public bool Init()
        {
            try
            {
                var config = new ConsumerConfig { GroupId = $"group-{_options.CurrentValue.Topic}", BootstrapServers = _options.CurrentValue.BrokerAddress, EnableAutoCommit = false };

                _consumer = new ConsumerBuilder<Null, string>(config).SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}")).Build();
                _logger.LogInformation("Consumer created");

                _timer = new System.Timers.Timer(_options.CurrentValue.ConsumeInterval * 1000);
                _timer.AutoReset = true;
                _timer.Elapsed += _timer_Elapsed;

                _logger.LogInformation("Consumer has Initialized");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Init");
                return false;
            }
        }

        public void Start()
        {
            _cancelToken = _cancellationTokenSource.Token;
            Task.Run(() => DoWork(), _cancelToken);
            _timer.Start();
            _logger.LogInformation("KafkaConsumer Timer Started");
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _consumer.Close();
            _timer.Stop();
            _logger.LogInformation("KafkaConsumer Timer Stopped");
        }


        public void DoWork()
        {
            _consumer.Subscribe(_options.CurrentValue.Topic);

            while (_cancelToken.IsCancellationRequested == false)
            {
                try
                {
                    var eventHistoryMessage = GetFromKafka();
                    if (eventHistoryMessage != null)
                    {
                        messagesQueue.Enqueue(eventHistoryMessage);
                        _logger.LogInformation("Message added to local queue");
                    }
                    else
                        _logger.LogWarning("EventHistoryMessage has null value");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in DoWork!!!");
                }
            }
        }

        public ConsumeResult<Null, string> GetFromKafka()
        {
            _logger.LogInformation("Ready to Consume");

            var consumeResult = _consumer.Consume(_cancelToken);

            _logger.LogInformation($"Received message at {consumeResult.TopicPartitionOffset}: ${consumeResult.Message.Value}");

            return consumeResult;
        }

        private async void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            _logger.LogInformation("Timer checking started");
            try
            {
                var messageCount = messagesQueue.Count;
                _logger.LogInformation($"Message count on local queue is {messageCount}");

                if (messageCount > 0)
                {
                    var consumeResults = DequeueMessages(messageCount);
                    if (consumeResults.Any())
                    {
                        var histories = consumeResults.Select(c => JsonConvert.DeserializeObject<EventHistoryCheckoutEvent>(c.Message.Value)).ToList();
                        var eventHistories = MapHistories(histories, _cacheService);
                        bool result = false;
                        try
                        {
                            await _eventHistoryRepository.BulkInsert(eventHistories);
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error on Bulk Insert");
                            result = false;
                        }

                        if (result == true)
                        {
                            foreach (var consumeResult in consumeResults)
                            {
                                _consumer.Commit(consumeResult);
                            }
                        }
                        else
                            _logger.LogWarning($"Data did not save on Db {consumeResults.Count}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Timer_Elapsed has Error");
            }

            _logger.LogInformation("Timer checking stopped");
            _timer.Enabled = true;
        }


        private List<ConsumeResult<Null, string>> DequeueMessages(int count)
        {
            var result = new List<ConsumeResult<Null, string>>();
            for (int i = 0; i < count; i++)
            {
                if (messagesQueue.TryDequeue(out var message))
                {
                    result.Add(message);
                }
            }

            return result;
        }

        private static List<EventHistory> MapHistories(List<EventHistoryCheckoutEvent> eventHistories, ICacheService cacheService)
        {
            return eventHistories.Select(item => new EventHistory
            {
                UserId = item.UserId.HasValue == true ? item.UserId.Value : cacheService.GetUserId(item.TagCode),
                TagCode = item.TagCode,
                SourceType = (SourceType)item.SourceType,
                OfficeId = item.OfficeId,
                DoorId = item.DoorId,
                OperationResult = item.OperationResult,
                CreatedDate = item.CreatedDate
            }).ToList();
        }
    }
}
