using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Door.Queries.GetDoor;
using ClayService.Domain.Entities;
using ClayService.Infrastructure.Persistence;
using ClayService.Infrastructure.Repositories;
using ClayService.Infrastructure.UnitTests.Extensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel.Exceptions;
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
        private Mock<DbSet<Office>> _mockOfficeSet;
        private Mock<ClayServiceDbContext> _mockContext;
        private Mock<IDateTimeService> _dateTimeService;
        private Mock<IDeviceService> _deviceService;
        private Mock<IKafkaProducer> _kafkaProducer;
        private Mock<ILogger<DoorRepository>> _logger;

        public DoorRepositoryTests()
        {
            _mockContext = new Mock<ClayServiceDbContext>();
            _mockDoorSet = new Mock<DbSet<Door>>();
            _mockOfficeSet = new Mock<DbSet<Office>>();
            _dateTimeService = new Mock<IDateTimeService>();
            _deviceService = new Mock<IDeviceService>();
            _kafkaProducer = new Mock<IKafkaProducer>();
            _logger = new Mock<ILogger<DoorRepository>>();
        }

        #region Create

        [Fact]
        public async void Create_ValidDoor_ValidOffice_AddedToContext()
        {
            ////Arrange            
            //var commnad = new CreateDoorCommand { Name = "Test", OfficeId = 1 };
            //var office = new Office { Id = 1, Title = "Office_1", IsDeleted = false, CreatedDate = DateTime.Now };
            //var entities = DoorEntities(office);

            //_dateTimeService.Setup(d => d.Now).Returns(() => DateTime.Now);
            //_mockDoorSet.IqueryableRegisteration(entities.AsQueryable());
            //_mockContext.Setup(m => m.Doors).Returns(_mockDoorSet.Object);
            //_mockContext.Setup(o => o.offices).Returns(_mockOfficeSet.Object);
            //_mockOfficeSet.Setup(o => o.FindAsync(office.Id)).ReturnsAsync(office);

            //var sut = new DoorRepository(_mockContext.Object, _deviceService.Object, _dateTimeService.Object, _kafkaProducer.Object, _logger.Object);

            ////Act
            //var result = await sut.CreateAsync(commnad);

            ////Assert
            //_mockDoorSet.Verify(m => m.AddAsync(It.IsAny<Door>(), It.IsAny<CancellationToken>()), Times.Once());
            //_mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Create_DuplicateDoor_ThrewBadRequestException()
        {
            ////Arrange            
            //var commnad = new CreateDoorCommand { Name = "Door_1", OfficeId = 1 };
            //var office = new Office { Id = 1, Title = "Office_1", IsDeleted = false, CreatedDate = DateTime.Now };
            //var entities = DoorEntities(office);

            //_dateTimeService.Setup(d => d.Now).Returns(() => DateTime.Now);
            //_mockDoorSet.IqueryableRegisteration(entities.AsQueryable());
            //_mockContext.Setup(m => m.Doors).Returns(_mockDoorSet.Object);

            //var sut = new DoorRepository(_mockContext.Object, _deviceService.Object, _dateTimeService.Object, _kafkaProducer.Object, _logger.Object);

            ////Act
            //Func<Task> result = async () => { await sut.CreateAsync(commnad); };

            ////Assert
            //result.Should().ThrowAsync<BadRequestException>();
        }

        #endregion

        #region Get

        [Fact]
        public async void Get_ValidDoorId_ReturnEntity()
        {
            ////Arrange            
            //var query = new GetDoorQuery { DoorId = 1 };
            //var office = new Office { Id = 1, Title = "Office_1", IsDeleted = false, CreatedDate = DateTime.Now };
            //_mockDoorSet.IqueryableRegisteration(DoorEntities(office).AsQueryable());
            //_mockContext.Setup(m => m.Doors).Returns(_mockDoorSet.Object);

            //var sut = new DoorRepository(_mockContext.Object, _deviceService.Object, _dateTimeService.Object, _kafkaProducer.Object, _logger.Object);

            ////Act
            //var result = await sut.GetAsync(query);

            ////Assert
            //result.Should().NotBeNull();
            //result.Id.Should().Be(query.DoorId);
        }

        [Fact]
        public void Get_InvalidDoorId_ThrewNotFoundException()
        {
            ////Arrange            
            //var query = new GetDoorQuery { DoorId = -1 };
            //var office = new Office { Id = 1, Title = "Office_1", IsDeleted = false, CreatedDate = DateTime.Now };
            //_mockDoorSet.IqueryableRegisteration(DoorEntities(office).AsQueryable());
            //_mockContext.Setup(m => m.Doors).Returns(_mockDoorSet.Object);

            //var sut = new DoorRepository(_mockContext.Object, _deviceService.Object, _dateTimeService.Object, _kafkaProducer.Object, _logger.Object);

            ////Act
            //Func<Task> result = async () => { await sut.GetAsync(query); };

            ////Assert
            //result.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region Privates

        private static IEnumerable<Door> DoorEntities(Office office)
        {
            return new List<Door>
            {
                new Door
                {
                    Id = 1,
                    Name = "Door_1",
                    Office = office,
                    OfficeId = office.Id,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                },
                new Door
                {
                    Id = 2,
                    Name = "Door_2",
                    Office = office,
                    OfficeId = office.Id,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                },
                new Door
                {
                    Id = 3,
                    Name = "Door3",
                    Office = office,
                    OfficeId = office.Id,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                }
            };
        }

        #endregion
    }
}
