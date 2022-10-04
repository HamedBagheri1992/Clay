using ClayService.Application.Common.Settings;
using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using Confluent.Kafka;
using EventBus.Messages.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClayService.Infrastructure.Services
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly IEventHistoryRepository _eventHistoryRepository;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IOptionsMonitor<KafkaSettingsConfigurationModel> _options;
        private IConsumer<Ignore, string> _consumer;
        private System.Timers.Timer _timer;

        public KafkaConsumer(IEventHistoryRepository eventHistoryRepository, IOptionsMonitor<KafkaSettingsConfigurationModel> options, ILogger<KafkaConsumer> logger)
        {
            _eventHistoryRepository = eventHistoryRepository;
            _options = options;
            _logger = logger;
        }

        public bool Init()
        {
            try
            {
                var config = new ConsumerConfig { GroupId = new Guid().ToString(), BootstrapServers = _options.CurrentValue.BrokerAddress, EnableAutoCommit = false };

                _consumer = new ConsumerBuilder<Ignore, string>(config).SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}")).Build();
                _logger.LogInformation("Consumer created");

                _timer = new System.Timers.Timer(_options.CurrentValue.ConsumeInterval * 1000);
                _timer.Elapsed += _timer_Elapsed;

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
            _timer.Start();
            _logger.LogInformation("KafkaConsumer Timer Started");
        }

        public void Stop()
        {
            _consumer.Close();
            _timer.Stop();
            _logger.LogInformation("KafkaConsumer Timer Stopped");
        }


        private async void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            try
            {
                _logger.LogInformation("Timer checking");

                var consumeResults = ReadMessage();
                if (consumeResults.Any())
                {
                    var histories = consumeResults.Select(c => JsonConvert.DeserializeObject<EventHistoryCheckoutEvent>(c.Message.Value)).ToList();
                    var result = await _eventHistoryRepository.BulkInsert(histories);
                    if (result == true)
                    {
                        foreach (var consumeResult in consumeResults)
                        {
                            _consumer.Commit(consumeResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Timer_Elapsed has Error");
            }

            _timer.Start();
        }


        private List<ConsumeResult<Ignore, string>> ReadMessage()
        {
            var consumeResults = new List<ConsumeResult<Ignore, string>>();
            _consumer.Assign(new TopicPartitionOffset(_options.CurrentValue.Topic, 0, Offset.Stored));

            while (true)
            {
                try
                {
                    _logger.LogInformation("Consume");

                    var consumeResult = _consumer.Consume();
                    if (consumeResult.IsPartitionEOF)
                        break;

                    consumeResults.Add(consumeResult);
                    _logger.LogInformation($"Received message at {consumeResult.TopicPartitionOffset}: ${consumeResult.Message.Value}");
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, $"Consume error: {ex.Error.Reason}");
                }
            }

            return consumeResults;
        }
    }
}
