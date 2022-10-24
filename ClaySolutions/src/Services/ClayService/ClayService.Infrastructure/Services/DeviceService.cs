using ClayService.Application.Contracts.Infrastructure;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Services
{
    public class DeviceService : IDeviceService
    {
        public async Task<bool> SendCommand(string TagCode)
        {
            return await Task.FromResult(true);
        }
    }
}
