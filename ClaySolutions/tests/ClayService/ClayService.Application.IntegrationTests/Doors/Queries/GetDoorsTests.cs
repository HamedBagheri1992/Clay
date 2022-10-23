using ClayService.Application.Features.Door.Queries.GetDoors;
using ClayService.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClayService.Application.IntegrationTests.Doors.Queries;
using static Testing;
public class GetDoorsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnDoorsPaginated()
    {
        await AddAsync(new Office
        {
            Title = "New Office",
            Doors = new List<Door>()
            {
                new Door { Name = "Door 1" },
                new Door { Name = "Door 2" },
                new Door { Name = "Door 3" },
                new Door { Name = "Door 4" },
                new Door { Name = "Door 5" }
            }
        });

        var query = new GetDoorsQuery { PageNumber = 1, PageSize = 15 };

        var paginatedDoors = await SendAsync(query);

        paginatedDoors.Should().NotBeNull();
        paginatedDoors.Pagination.Should().NotBeNull();
        paginatedDoors.Pagination.HasNext.Should().BeFalse();
        paginatedDoors.Pagination.HasPrevious.Should().BeFalse();
        paginatedDoors.Items.Should().HaveCount(5);
    }
}
