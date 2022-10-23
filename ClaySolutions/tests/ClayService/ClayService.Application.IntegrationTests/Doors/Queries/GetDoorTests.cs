using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Door.Queries.GetDoor;
using ClayService.Application.Features.Office.Commands.CreateOffice;
using FluentAssertions;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;

namespace ClayService.Application.IntegrationTests.Doors.Queries;
using static Testing;
public class GetDoorTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidDoor()
    {
        var query = new GetDoorQuery { DoorId = 1 };

        await FluentActions.Invoking(() => SendAsync(query)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldReturnDoor()
    {
        var office = await SendAsync(new CreateOfficeCommand
        {
            Title = "New Office"
        });

        var door = await SendAsync(new CreateDoorCommand
        {
            OfficeId = office.Id,
            Name = "New Door"
        });

        var query = new GetDoorQuery { DoorId = door.Id };

        var doorDto = await SendAsync(query);

        doorDto.Should().NotBeNull();
        doorDto.Id.Should().Be(query.DoorId);
        doorDto.Name.Should().Be(door.Name);
        doorDto.OfficeId.Should().Be(door.OfficeId);
    }
}
