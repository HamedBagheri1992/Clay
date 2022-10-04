namespace ClayService.Application.Contracts.Infrastructure
{
    public interface IKafkaConsumer
    {
        bool Init();
        void Start();
        void Stop();
    }
}
