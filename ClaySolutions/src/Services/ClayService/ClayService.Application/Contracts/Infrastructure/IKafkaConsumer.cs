namespace ClayService.Application.Contracts.Infrastructure
{
    public interface IKafkaConsumer
    {
        void Start();
        void Stop();
    }
}
