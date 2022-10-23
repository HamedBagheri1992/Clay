namespace ClayService.Application.IntegrationTests;

using System.Threading.Tasks;
using static Testing;

public abstract class BaseTestFixture
{
    public async Task TestSetUp()
    {
        await ResetState();
    }
}
