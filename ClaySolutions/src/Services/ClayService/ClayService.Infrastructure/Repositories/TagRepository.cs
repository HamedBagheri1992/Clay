using ClayService.Application.Contracts.Infrastructure;
using ClayService.Application.Contracts.Persistence;
using ClayService.Application.Features.Tag.Commands.AssignTag;
using ClayService.Application.Features.Tag.Commands.CreateTag;
using ClayService.Application.Features.Tag.Queries.GetTag;
using ClayService.Application.Features.Tag.Queries.GetTags;
using ClayService.Application.Features.Tag.Queries.MyTag;
using ClayService.Domain.Entities;
using ClayService.Infrastructure.Persistence;
using EventBus.Messages.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel.Common;
using SharedKernel.Exceptions;
using SharedKernel.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClayService.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ClayServiceDbContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICacheService _cacheService;

        public TagRepository(ClayServiceDbContext context, IDateTimeService dateTimeService, ICacheService cacheService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _cacheService = cacheService;
        }

        public async Task<PhysicalTag> GetAsync(GetTagQuery request)
        {
            var tag = await _context.PhysicalTags.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);
            if (tag == null)
                throw new NotFoundException(nameof(tag), request.Id);

            return tag;
        }

        public async Task<PaginatedResult<PhysicalTag>> GetAsync(GetTagsQuery request)
        {
            var query = _context.PhysicalTags.AsNoTracking().AsQueryable();

            if (string.IsNullOrEmpty(request.TagCode) == false)
                query = query.Where(t => t.TagCode.Contains(request.TagCode));

            if (request.StartCreatedDate.HasValue)
                query = query.Where(t => t.CreatedDate >= request.StartCreatedDate.Value);

            if (request.EndCreatedDate.HasValue)
                query = query.Where(t => t.CreatedDate <= request.EndCreatedDate.Value);

            return await query.ToPagedListAsync(request.PageNumber, request.PageSize);
        }

        public async Task<PhysicalTag> GetAsync(MyTagQuery request)
        {
            var currentUser = await _context.Users.Include(u => u.PhysicalTag).AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (currentUser == null)
                throw new NotFoundException(nameof(currentUser), request.UserId);

            if (currentUser.PhysicalTag == null)
                throw new NotFoundException(nameof(PhysicalTag), request.UserId);

            return currentUser.PhysicalTag;
        }

        public async Task<PhysicalTag> CreateAsync(CreateTagCommand request)
        {
            if (await _context.PhysicalTags.AnyAsync(t => t.TagCode == request.TagCode) == true)
                throw new BadRequestException("TagCode is duplicate");

            var tag = new PhysicalTag { TagCode = request.TagCode, CreatedDate = _dateTimeService.Now };
            await _context.PhysicalTags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return tag;
        }

        public async Task AssignTagToUserAsync(AssignTagCommand request)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = await _context.Users.FindAsync(request.UserId);
                    if (user == null)
                        throw new BadRequestException("UserId is Invalid");

                    var tag = await _context.PhysicalTags.FindAsync(request.UserId);
                    if (tag == null)
                        throw new BadRequestException("UserId is Invalid");

                    if (request.RemoveRequest == true)
                        user.PhysicalTagId = null;
                    else
                        user.PhysicalTagId = tag.Id;

                    await _context.SaveChangesAsync();

                    if (request.RemoveRequest == true)
                        await _cacheService.DeleteTagAsync(tag.TagCode);
                    else
                    {
                        var result = await _cacheService.AddOrUpdateTagAsync(tag.TagCode, user.Id);
                        if (result.HasValue == false)
                            throw new ApiException("Error on Cache server...");
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new ApiException(ex.Message);
                }
            }
        }
    }
}
