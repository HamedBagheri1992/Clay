using Microsoft.EntityFrameworkCore;
using SSO.Application.Contracts.Persistence;
using SSO.Domain.Entities;
using SSO.Infrastructure.Persistence;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly SSODbContext _context;

        public RefreshTokenRepository(SSODbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken> GetLatestOneAsync(long userId)
        {
            return await _context.RefreshTokens.OrderByDescending(o => o.Id).FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _context.Entry(refreshToken).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
