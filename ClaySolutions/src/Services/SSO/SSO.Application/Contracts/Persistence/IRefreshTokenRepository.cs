using SSO.Domain.Entities;
using System.Threading.Tasks;

namespace SSO.Application.Contracts.Persistence
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> AddAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetLatestOneAsync(long userId);
        Task UpdateAsync(RefreshToken lastRefreshToken);
    }
}
