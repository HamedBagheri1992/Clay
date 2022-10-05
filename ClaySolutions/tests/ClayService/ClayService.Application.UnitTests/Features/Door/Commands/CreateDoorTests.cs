using AutoMapper;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Mappings;
using FluentAssertions;
using Moq;
using SharedKernel.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ClayService.Application.UnitTests.Features.Door.Commands
{
    public class CreateDoorTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IDoorRepository> _mockDoorRepository;

        public CreateDoorTests()
        {
            _mockDoorRepository = new Mock<IDoorRepository>();
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
            _mockDoorRepository.Setup(repo => repo.CreateAsync(command)).ReturnsAsync(new Domain.Entities.Door() { Name = command.Name, OfficeId = command.OfficeId });
            var handler = new CreateDoorCommandHandler(_mockDoorRepository.Object, _mapper);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Asert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Handle_DuplicateDoor_ThrewBadRequestException()
        {
            //Arrange          
            var command = new CreateDoorCommand { Name = "Test", OfficeId = 1 };
            _mockDoorRepository.Setup(repo => repo.CreateAsync(command)).Throws(new BadRequestException("Door Name is duplicate in an office"));
            var handler = new CreateDoorCommandHandler(_mockDoorRepository.Object, _mapper);

            //Act
            Func<Task> result = async () => { await handler.Handle(command, CancellationToken.None); };

            //Asert            
            result.Should().ThrowAsync<BadRequestException>();
        }
    }
}
