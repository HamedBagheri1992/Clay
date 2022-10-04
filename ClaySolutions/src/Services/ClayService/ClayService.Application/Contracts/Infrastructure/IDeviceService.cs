using System;
using System.Threading.Tasks;

namespace ClayService.Application.Contracts.Infrastructure
{
    public interface IDeviceService
    {
        Task<bool> SendCommand(Guid TagCode);
    }
}
