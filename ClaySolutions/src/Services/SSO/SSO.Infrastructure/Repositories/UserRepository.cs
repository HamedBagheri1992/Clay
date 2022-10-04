using EventBus.Messages.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel.Common;
using SharedKernel.Exceptions;
using SharedKernel.Extensions;
using SSO.Application.Contracts.Infrastructure;
using SSO.Application.Contracts.Persistence;
using SSO.Application.Features.User.Commands.CreateUser;
using SSO.Application.Features.User.Commands.DeleteUser;
using SSO.Application.Features.User.Commands.UpdateUser;
using SSO.Application.Features.User.Queries.GetUser;
using SSO.Application.Features.User.Queries.GetUsers;
using SSO.Domain.Entities;
using SSO.Infrastructure.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SSODbContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserRepository(SSODbContext context, IEncryptionService encryptionService, IDateTimeService dateTimeService, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _encryptionService = encryptionService;
            _dateTimeService = dateTimeService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<PaginatedResult<User>> GetAsync(GetUsersQuery request)
        {
            var users = _context.Users.AsNoTracking().AsQueryable();

            if (string.IsNullOrEmpty(request.UserName) == false)
                users = users.Where(u => u.UserName.Contains(request.UserName));

            if (string.IsNullOrEmpty(request.FirstName) == false)
                users = users.Where(u => u.FirstName.Contains(request.FirstName));

            if (string.IsNullOrEmpty(request.LastName) == false)
                users = users.Where(u => u.LastName.Contains(request.LastName));

            if (request.IsActive.HasValue == true)
                users = users.Where(u => u.IsActive == request.IsActive.Value);

            return await users.ToPagedListAsync(request.PageNumber, request.PageSize);
        }

        public async Task<User> GetAsync(GetUserQuery request)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.Id);
            if (user == null)
                throw new NotFoundException(nameof(user), request.Id);

            return user;
        }

        public async Task<User> CreateAsync(CreateUserCommand request)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName) == true)
                throw new BadRequestException("UserName is Duplicated, Please select another.");


            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = new User()
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        UserName = request.UserName,
                        Password = _encryptionService.HashPassword(request.Password),
                        IsActive = request.IsActive,
                        IsDeleted = false,
                        CreatedDate = _dateTimeService.Now
                    };

                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    var userCheckoutEvent = new UserCheckoutEvent { UserId = user.Id, UserName = user.UserName, DisplayName = $"{user.FirstName} {user.LastName}" };
                    await _publishEndpoint.Publish(userCheckoutEvent);

                    transaction.Commit();

                    return user;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new ApiException(ex.Message);
                }
            }
        }

        public async Task UpdateAsync(UpdateUserCommand request)
        {
            var user = await _context.Users.FindAsync(request.Id);
            if (user == null)
                throw new NotFoundException(nameof(user), request.Id);

            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.IsActive = request.IsActive;

                    await _context.SaveChangesAsync();

                    var userCheckoutEvent = new UserCheckoutEvent { UserId = user.Id, UserName = user.UserName, DisplayName = $"{user.FirstName} {user.LastName}" };
                    await _publishEndpoint.Publish(userCheckoutEvent);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new ApiException(ex.Message);
                }
            }
        }

        public async Task DeleteAsync(DeleteUserCommand request)
        {
            var user = await _context.Users.FindAsync(request.Id);
            if (user == null)
                throw new NotFoundException(nameof(user), request.Id);

            user.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
