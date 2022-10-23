using ClayService.Application.Features.Door.Commands.AssignDoor;
using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Door.Queries.MyDoors;
using ClayService.Application.Features.Office.Commands.CreateOffice;
using ClayService.Application.Features.User.Commands.UserAddOrUpdate;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Application.IntegrationTests.Doors.Queries;
using static Testing;
public class MyDoorsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnSpecificUserDoors()
    {
        long userId = 2;
        await SendAsync(new UserAddOrUpdateCommand
        {
            Id = userId,
            DisplayName = "Test Test",
            UserName = "Test"
        });

        var office = await SendAsync(new CreateOfficeCommand
        {
            Title = "New Office"
        });

        var door = await SendAsync(new CreateDoorCommand
        {
            OfficeId = office.Id,
            Name = "New Door"
        });

        await SendAsync(new AssignDoorCommand(userId, door.Id, true, 1));

        var query = new MyDoorsQuery(userId);
        var item = await SendAsync(query);

        item.Should().NotBeNull();
        item.Should().HaveCount(1);
        item.First().Name.Should().Be(door.Name);
        item.First().OfficeId.Should().Be(door.OfficeId);
    }
}

