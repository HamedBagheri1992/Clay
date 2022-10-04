using ClayService.Application.Common.Settings;
using ClayService.Application.Contracts.Infrastructure;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Services
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IOptionsMonitor<KafkaSettingsConfigurationModel> _options;
        private readonly IProducer<string, string> _producer;

        public KafkaProducer(IOptionsMonitor<KafkaSettingsConfigurationModel> options)
        {
            _options = options;
            var config = new ProducerConfig { BootstrapServers = options.CurrentValue.BrokerAddress };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task WriteMessageAsync(string message)
        {
            await _producer.ProduceAsync(_options.CurrentValue.Topic, new Message<string, string> { Key = null, Value = message });
        }
    }
}
