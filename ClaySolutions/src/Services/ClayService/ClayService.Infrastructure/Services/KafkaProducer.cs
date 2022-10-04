using ClayService.Application.Common.Settings;
using ClayService.Application.Contracts.Infrastructure;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Services
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IOptionsMonitor<KafkaSettingsConfigurationModel> _options;
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(IOptionsMonitor<KafkaSettingsConfigurationModel> options, ILogger<KafkaProducer> logger)
        {
            _logger = logger;
            _options = options;
            var config = new ProducerConfig { BootstrapServers = options.CurrentValue.BrokerAddress };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task WriteMessageAsync(string message)
        {
            _logger.LogInformation($"Produce Message Start Sending => {message}");

            await _producer.ProduceAsync(_options.CurrentValue.Topic, new Message<Null, string> { Value = message });

            _logger.LogInformation($"Produce Message Sending Finished => {message}");
        }
    }
}
