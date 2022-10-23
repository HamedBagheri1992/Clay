using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Door.Commands.UpdateDoor;
using ClayService.Application.Features.Office.Commands.CreateOffice;
using ClayService.Domain.Entities;
using FluentAssertions;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Xunit;

namespace ClayService.Application.IntegrationTests.Doors.Commands;
using static Testing;

public class UpdateDoorTests : BaseTestFixture
{
    public UpdateDoorTests()
    {
        new Testing().RunBeforeAnyTests();
        TestSetUp().Wait();
    }

    [Fact]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new UpdateDoorCommand();

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShouldRequireUniqueName()
    {
        var office = await SendAsync(new CreateOfficeCommand
        {
            Title = "New Office"
        });

        var door1 = await SendAsync(new CreateDoorCommand
        {
            OfficeId = office.Id,
            Name = "New Door 1"
        });

        var door2 = await SendAsync(new CreateDoorCommand
        {
            OfficeId = office.Id,
            Name = "New Door 2"
        });


        var command = new UpdateDoorCommand
        {
            Id = door2.Id,
            OfficeId = door2.OfficeId,
            Name = door1.Name
        };

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShouldRequireValidOffice()
    {
        var command = new UpdateDoorCommand
        {
            Id = 1,
            OfficeId = 1,
            Name = "Door"
        };

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShouldUpdateDoor()
    {
        var office = await SendAsync(new CreateOfficeCommand
        {
            Title = "New Office"
        });

        var door1 = await SendAsync(new CreateDoorCommand
        {
            OfficeId = office.Id,
            Name = "New Door 1"
        });

        var command = new UpdateDoorCommand
        {
            Id = door1.Id,
            OfficeId = door1.OfficeId,
            Name = "New Name"
        };

        await SendAsync(command);

        var item = await FindAsync<Door>(door1.Id);

        item.Should().NotBeNull();
        item!.OfficeId.Should().Be(command.OfficeId);
        item.Name.Should().Be(command.Name);        
    }
}