using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Door.Commands.AssignDoor;
using ClayService.Application.Features.Door.Commands.CreateDoor;
using ClayService.Application.Features.Door.Commands.OperationDoor;
using ClayService.Application.Features.Door.Queries.GetDoor;
using ClayService.Application.Features.Door.Queries.GetDoors;
using ClayService.Application.Features.Door.Queries.MyDoors;
using ClayService.Domain.Entities;
using ClayService.Domain.Enums;
using ClayService.Infrastructure.Persistence;
using EventBus.Messages.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedKernel.Common;
using SharedKernel.Exceptions;
using SharedKernel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Repositories
{
    public class DoorRepository : IDoorRepository
    {
        private readonly ClayServiceDbContext _context;
        private readonly IDeviceService _deviceService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<DoorRepository> _logger;

        public DoorRepository(ClayServiceDbContext context, IDeviceService deviceService, IDateTimeService dateTimeService, IKafkaProducer kafkaProducer, ILogger<DoorRepository> logger)
        {
            _context = context;
            _deviceService = deviceService;
            _dateTimeService = dateTimeService;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task<Door> GetAsync(GetDoorQuery request)
        {
            var door = await _context.Doors.AsNoTracking().FirstOrDefaultAsync(d => d.Id == request.DoorId);
            if (door == null)
                throw new NotFoundException(nameof(door), request.DoorId);

            return door;
        }

        public async Task<PaginatedResult<Door>> GetAsync(GetDoorsQuery request)
        {
            var query = _context.Doors.AsNoTracking().AsQueryable();

            if (string.IsNullOrEmpty(request.Name) == false)
                query = query.Where(d => d.Name.Contains(request.Name));

            if (request.OfficeId.HasValue == true)
                query = query.Where(d => d.OfficeId == request.OfficeId.Value);


            if (request.IsActive.HasValue == true)
                query = query.Where(d => d.IsActive == request.IsActive.Value);
            else
                query = query.Where(d => d.IsActive == true);

            return await query.ToPagedListAsync(request.PageNumber, request.PageSize);
        }

        public async Task<List<Door>> GetAsync(MyDoorsQuery request)
        {
            return await _context.Users.Include(u => u.Doors).AsNoTracking().Where(u => u.Id == request.UserId).SelectMany(d => d.Doors).ToListAsync();
        }

        public async Task<Door> CreateAsync(CreateDoorCommand request)
        {
            var office = await _context.offices.FindAsync(request.OfficeId);

            var door = new Door
            {
                Name = request.Name,
                Office = office,
                OfficeId = request.OfficeId,
                IsActive = true,
                CreatedDate = _dateTimeService.Now
            };

            await _context.Doors.AddAsync(door);
            await _context.SaveChangesAsync();

            return door;
        }

        public async Task OperationAsync(OperationDoorCommand request)
        {
            var currentUser = await _context.Users.Include(u => u.PhysicalTag).Include(u => u.Doors).AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (currentUser == null)
                throw new NotFoundException(nameof(User), request.UserId);

            if (currentUser.PhysicalTagId.HasValue == false)
                throw new BadRequestException("You do not have PhysicalTag");

            if (currentUser.Doors.Count == 0)
            {
                _logger.LogError($"User {currentUser.UserName} does not have access to Door {request.DoorId}");
                throw new BadRequestException("Access denied to door");
            }

            var door = currentUser.Doors.FirstOrDefault(d => d.Id == request.DoorId);
            if (door == null)
                throw new NotFoundException(nameof(door), request.DoorId);

            //Send To Door

            bool operationResult = await _deviceService.SendCommand(currentUser.PhysicalTag.TagCode);

            //Log
            var eventHistoryCheckout = new EventHistoryCheckoutEvent()
            {
                UserId = request.UserId,
                TagCode = currentUser.PhysicalTag.TagCode,
                OperationResult = operationResult,
                OfficeId = door.OfficeId,
                DoorId = door.Id,
                SourceType = (byte)SourceType.Appication,
                CreatedDate = _dateTimeService.Now
            };

            await KafkaProduceMessage(eventHistoryCheckout);

            if (operationResult == false)
            {
                _logger.LogInformation($"The Opeartion failed for Door {door.Name} by User {currentUser.UserName}");
                throw new ApiException("The Operation failed");
            }
        }

        public async Task AssignDoorToUserAsync(AssignDoorCommand request)
        {
            var user = await _context.Users.Include(u => u.Doors).FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            var offices = await _context.Doors.Include(d => d.Office).Where(d => request.DoorIds.Contains(d.Id)).Select(o => o.Office).Distinct().ToListAsync();
            if (offices.Count != 1)
                throw new BadRequestException("DoorIds must be for a single office");

            var office = offices.First();
            if (user.Offices.Any(o => o.Id == office.Id) == false)
                user.Offices.Add(office);

            var (deleteDoors, addDoorIds) = ConsistencyUserDoor(user.Doors, request.DoorIds);

            var addDoors = await _context.Doors.Where(d => addDoorIds.Contains(d.Id)).ToListAsync();
            if (addDoorIds.Count != addDoors.Count)
                throw new BadRequestException("Some DoorIds are Invalid.");

            user.Doors.AddRange(addDoors);
            user.Doors.RemoveRange(deleteDoors);

            await _context.SaveChangesAsync();
        }

        #region Privates

        private (List<Door> deleteDoors, List<long> addDoorIds) ConsistencyUserDoor(IEnumerable<Door> doors, List<long> requestDoorIds)
        {
            var deleteDoors = doors.Where(dd => requestDoorIds.All(d => d != dd.Id)).ToList();
            var addDoorIds = requestDoorIds.Where(dd => doors.All(d => d.Id != dd)).ToList();

            return (deleteDoors, addDoorIds);
        }

        private async Task KafkaProduceMessage(EventHistoryCheckoutEvent eventHistoryCheckout)
        {
            try
            {
                string message = JsonConvert.SerializeObject(eventHistoryCheckout);
                await _kafkaProducer.WriteMessageAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "KafkaProducer has error");
            }
        }

        #endregion
    }
}
