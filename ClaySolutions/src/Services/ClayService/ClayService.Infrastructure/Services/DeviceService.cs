using ClayService.Application.Contracts.Infrastructure;
using System;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Services
{
    public class DeviceService : IDeviceService
    {
        public async Task<bool> SendCommand(string TagCode)
        {
            var random = new Random();
            await Task.Delay(200);
            return random.Next() % 50 != 0;
        }
    }
}
