using ClayService.Application.Features.Door.Commands.AssignDoor;
using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Office.Commands.CreateOffice;
using ClayService.Application.Features.User.Commands.UserAddOrUpdate;
using ClayService.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;

namespace ClayService.Application.IntegrationTests.Doors.Commands;
using static Testing;

public class AssignDoorTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new AssignDoorCommand(0, 0, true, 1);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidUser()
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

        var command = new AssignDoorCommand(2, door.Id, true, 2);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireValidDoor()
    {
        long userId = 2;
        await SendAsync(new UserAddOrUpdateCommand
        {
            Id = userId,
            DisplayName = "Test Test",
            UserName = "Test"
        });

        var command = new AssignDoorCommand(userId, 1, false, userId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireAccessToOffice()
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

        var command = new AssignDoorCommand(userId, door.Id, false, userId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<BadRequestException>();
    }

    [Test]
    public async Task ShouldAssignDoorToUser()
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

        var command = new AssignDoorCommand(userId, door.Id, true, 1); //Id:1 is admin Id on Init(SeedData)

        await SendAsync(command);

        var item = await FirstOrDefaultAsync<Door>(door.Id);


        item.Should().NotBeNull();
        item.Name.Should().Be(door.Name);
        item.OfficeId.Should().Be(door.OfficeId);
        item.Users.Should().HaveCount(1);
    }
}
