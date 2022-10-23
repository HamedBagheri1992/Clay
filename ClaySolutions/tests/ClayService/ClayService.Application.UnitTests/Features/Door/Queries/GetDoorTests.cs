using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Door.Queries.GetDoor;
using ClayService.Application.Mappings;
using FluentAssertions;
using Moq;
using SharedKernel.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ClayService.Application.UnitTests.Features.Door.Queries
{
    public class GetDoorTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IDoorRepository> _mockDoorRepository;

        public GetDoorTests()
        {
            _mockDoorRepository = new Mock<IDoorRepository>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidDoorId_GetSingleDoor()
        {
            //Arrange          
            var query = new GetDoorQuery { DoorId = 1 };
            _mockDoorRepository.Setup(repo => repo.GetAsync(query.DoorId)).ReturnsAsync(new Domain.Entities.Door() { Id = query.DoorId, Name = "Test", OfficeId = 1 });
            var handler = new GetDoorQueryHandler(_mockDoorRepository.Object, _mapper);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Asert
            result.Should().NotBeNull();
            result.Should().BeOfType<DoorDto>();
            result.Id.Should().Be(query.DoorId);
        }

        [Fact]
        public async Task Handle_InvalidDoorId_ThrewNotFoundException()
        {
            //Arrange          
            var query = new GetDoorQuery { DoorId = -1 };
            _mockDoorRepository.Setup(repo => repo.GetAsync(query.DoorId)).Throws(new NotFoundException(nameof(Domain.Entities.Door), query.DoorId));
            var handler = new GetDoorQueryHandler(_mockDoorRepository.Object, _mapper);

            //Act
            var result = FluentActions.Invoking(() => handler.Handle(query, CancellationToken.None));

            //Asert            
            await result.Should().ThrowAsync<NotFoundException>();
        }
    }
}
