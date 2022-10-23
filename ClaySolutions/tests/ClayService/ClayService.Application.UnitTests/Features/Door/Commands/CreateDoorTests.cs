using AutoMapper;
using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Door.Queries.GetDoor;
using ClayService.Application.Mappings;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ClayService.Application.UnitTests.Features.Door.Commands
{
    public class CreateDoorTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IDoorRepository> _mockDoorRepository;
        private readonly Mock<IDateTimeService> _dateTimeService;

        public CreateDoorTests()
        {
            _mockDoorRepository = new Mock<IDoorRepository>();
            _dateTimeService = new Mock<IDateTimeService>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidDoor_AddedToDoorRepo()
        {
            //Arrange          
            var command = new CreateDoorCommand { Name = "Test", OfficeId = 1 };
            _mockDoorRepository.Setup(repo => repo.CreateAsync(It.IsAny<Domain.Entities.Door>())).ReturnsAsync(new Domain.Entities.Door() { Name = command.Name, OfficeId = command.OfficeId });
            var handler = new CreateDoorCommandHandler(_mockDoorRepository.Object, _mapper, _dateTimeService.Object);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Asert
            result.Should().NotBeNull();
            result.Should().BeOfType<DoorDto>();
            result.OfficeId.Should().Be(command.OfficeId);
            result.Name.Should().Be(command.Name);
        }
    }
}
