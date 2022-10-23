using ClayService.Domain.Entities;
using ClayService.Infrastructure.Persistence;
using ClayService.Infrastructure.Repositories;
using ClayService.Infrastructure.UnitTests.Extensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ClayService.Infrastructure.UnitTests.Repositories
{
    public class DoorRepositoryTests
    {
        private Mock<DbSet<Door>> _mockDoorSet;
        private Mock<ClayServiceDbContext> _mockContext;

        public DoorRepositoryTests()
        {
            _mockContext = new Mock<ClayServiceDbContext>();
            _mockDoorSet = new Mock<DbSet<Door>>();
        }

        #region Create

        [Fact]
        public async void Create_Door_AddedToContext()
        {
            //Arrange            
            var entities = DoorEntities();

            _mockDoorSet.IqueryableRegisteration(entities.AsQueryable());
            _mockContext.Setup(m => m.Doors).Returns(_mockDoorSet.Object);

            var sut = new DoorRepository(_mockContext.Object);

            //Act
            var result = await sut.CreateAsync(It.IsAny<Door>());

            //Assert
            _mockDoorSet.Verify(m => m.AddAsync(It.IsAny<Door>(), It.IsAny<CancellationToken>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Get

        [Fact]
        public async Task Get_ValidDoorId_ReturnEntity()
        {
            //Arrange            
            long id = 1;
            var entities = DoorEntities();
            _mockDoorSet.IqueryableRegisteration(entities.AsQueryable());
            _mockContext.Setup(m => m.Doors).Returns(_mockDoorSet.Object);
            _mockDoorSet.Setup(m => m.FindAsync(id)).ReturnsAsync(entities.First());

            var sut = new DoorRepository(_mockContext.Object);

            //Act
            var result = await sut.GetAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task Get_InvalidDoorId_ReturnNull()
        {
            //Arrange            
            long id = -1;
            var entities = DoorEntities();
            _mockDoorSet.IqueryableRegisteration(entities.AsQueryable());
            _mockContext.Setup(m => m.Doors).Returns(_mockDoorSet.Object);
            _mockDoorSet.Setup(m => m.FindAsync(id)).Returns(null);

            var sut = new DoorRepository(_mockContext.Object);

            //Act
            var result = await sut.GetAsync(id);

            //Assert
            result.Should().BeNull();
        }

        #endregion

        #region Privates

        private static IEnumerable<Door> DoorEntities()
        {
            return new List<Door>
            {
                new Door
                {
                    Id = 1,
                    Name = "Door_1",
                    OfficeId = 1,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                },
                new Door
                {
                    Id = 2,
                    Name = "Door_2",
                    OfficeId = 1,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                },
                new Door
                {
                    Id = 3,
                    Name = "Door3",
                    OfficeId = 1,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                }
            };
        }

        #endregion
    }
}
