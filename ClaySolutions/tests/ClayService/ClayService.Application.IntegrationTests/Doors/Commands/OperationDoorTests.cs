using ClayService.Application.Features.Door.Commands.AssignDoor;
using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Door.Commands.OperationDoor;
using ClayService.Application.Features.Office.Commands.CreateOffice;
using ClayService.Application.Features.Tag.Commands.AssignTag;
using ClayService.Application.Features.Tag.Commands.CreateTag;
using ClayService.Application.Features.User.Commands.UserAddOrUpdate;
using FluentAssertions;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;

namespace ClayService.Application.IntegrationTests.Doors.Commands;
using static Testing;
public class OperationDoorTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new OperationDoorCommand(0, 0);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidUser()
    {
        var command = new OperationDoorCommand(1, 2);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUserHavePhysicalTag()
    {
        long userId = 2;
        var user = await SendAsync(new UserAddOrUpdateCommand
        {
            Id = userId,
            UserName = " User 2",
            DisplayName = "User Test"
        });

        var command = new OperationDoorCommand(1, userId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<BadRequestException>();
    }

    [Test]
    public async Task ShouldRequireValidDoor()
    {
        long userId = 2;
        var user = await SendAsync(new UserAddOrUpdateCommand
        {
            Id = userId,
            UserName = " User 2",
            DisplayName = "User Test"
        });

        var tag = await SendAsync(new CreateTagCommand
        {
            TagCode = "Tag_1"
        });

        await SendAsync(new AssignTagCommand
        {
            TagId = tag.Id,
            UserId = userId,
            RemoveRequest = false
        });

        var command = new OperationDoorCommand(1, userId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireDoorIsAssigned()
    {
        long userId = 2;
        var user = await SendAsync(new UserAddOrUpdateCommand
        {
            Id = userId,
            UserName = " User 2",
            DisplayName = "User Test"
        });

        var tag = await SendAsync(new CreateTagCommand
        {
            TagCode = "Tag_1"
        });

        await SendAsync(new AssignTagCommand
        {
            TagId = tag.Id,
            UserId = userId,
            RemoveRequest = false
        });

        var office = await SendAsync(new CreateOfficeCommand
        {
            Title = "New Office"
        });

        var door = await SendAsync(new CreateDoorCommand
        {
            Name = "New Door",
            OfficeId = office.Id
        });

        var command = new OperationDoorCommand(door.Id, userId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<BadRequestException>();
    }

    [Test]
    public async Task ShouldOperationOnDoor()
    {
        long userId = 2;
        await SendAsync(new UserAddOrUpdateCommand
        {
            Id = userId,
            UserName = " User 2",
            DisplayName = "User Test"
        });

        var tag = await SendAsync(new CreateTagCommand
        {
            TagCode = "Tag_1"
        });

        await SendAsync(new AssignTagCommand
        {
            TagId = tag.Id,
            UserId = userId,
            RemoveRequest = false
        });

        var office = await SendAsync(new CreateOfficeCommand
        {
            Title = "New Office"
        });

        var door = await SendAsync(new CreateDoorCommand
        {
            Name = "New Door",
            OfficeId = office.Id
        });

        await SendAsync(new AssignDoorCommand(userId, door.Id, true, 1));

        var command = new OperationDoorCommand(door.Id, userId);
        await SendAsync(command);
    }
}