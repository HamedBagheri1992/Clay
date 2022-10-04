namespace ClayService.Application.Common.Settings
{
    public class KafkaSettingsConfigurationModel
    {

        public const string NAME = "KafkaSettings";
        public string BrokerAddress { get; set; }
        public string Topic { get; set; }
        public long ConsumeInterval { get; set; }
    }
}
