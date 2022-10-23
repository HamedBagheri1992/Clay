using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Office.Commands.CreateOffice;
using ClayService.Domain.Entities;
using FluentAssertions;
using SharedKernel.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ClayService.Application.IntegrationTests.Doors.Commands;
using static Testing;

public class CreateDoorTests : BaseTestFixture
{
    public CreateDoorTests()
    {
        new Testing().RunBeforeAnyTests();
        TestSetUp().Wait();
    }

    [Fact]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateDoorCommand();

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShouldRequireUniqueName()
    {
        var office = await SendAsync(new CreateOfficeCommand
        {
            Title = "New Office"
        });

        await SendAsync(new CreateDoorCommand
        {
            OfficeId = office.Id,
            Name = "New Door"
        });

        var command = new CreateDoorCommand
        {
            OfficeId = office.Id,
            Name = "New Door"
        };

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShouldRequireValidOffice()
    {
        var command = new CreateDoorCommand
        {
            OfficeId = 1,
            Name = "New Door"
        };

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShouldCreateDoor()
    {
        var office = await SendAsync(new CreateOfficeCommand
        {
            Title = "New Office"
        });

        var command = new CreateDoorCommand
        {
            OfficeId = office.Id,
            Name = "New Door"
        };

        var doorDto = await SendAsync(command);

        var item = await FindAsync<Door>(doorDto.Id);

        item.Should().NotBeNull();
        item!.OfficeId.Should().Be(command.OfficeId);
        item.Name.Should().Be(command.Name);
        item.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
