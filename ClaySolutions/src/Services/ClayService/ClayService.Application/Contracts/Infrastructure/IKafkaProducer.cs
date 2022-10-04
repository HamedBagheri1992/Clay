using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Infrastructure
{
    public interface IKafkaProducer
    {
        Task WriteMessageAsync(string message);
    }
}
