using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Exceptions;
using SharedKernel.Extensions;
using SSO.Application.Common.Settings;
using SSO.Application.Contracts.Infrastructure;
using SSO.Application.Contracts.Persistence;
using SSO.Application.Features.Account.Commands.ChangePassword;
using SSO.Application.Features.Account.Commands.UpdateUserRole;
using SSO.Application.Features.Account.Queries.Authenticate;
using SSO.Application.Features.Account.Queries.LogoutUser;
using SSO.Application.Features.Account.Queries.RefreshToken;
using SSO.Domain.Entities;
using SSO.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        protected readonly SSODbContext _context;
        private readonly IOptionsMonitor<BearerTokensConfigurationModel> _options;
        private readonly IEncryptionService _encryptionService;
        private readonly IDateTimeService _dateTimeService;

        public AccountRepository(SSODbContext context, IOptionsMonitor<BearerTokensConfigurationModel> options, IEncryptionService encryptionService, IDateTimeService dateTimeService)
        {
            _context = context;
            _options = options;
            _encryptionService = encryptionService;
            _dateTimeService = dateTimeService;
        }

        public async Task<AuthenticateDto> AuthenticateAsync(AuthenticateQuery request)
        {
            string encPass = _encryptionService.HashPassword(request.Password);
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.UserName == request.UserName && u.Password == encPass);
            if (user == null)
                throw new NotFoundException($"No Accounts Registered", request.UserName);

            if (user.IsDeleted == true)
                throw new BadRequestException($"Account is Deleted with {request.UserName}.");

            if (user.IsActive == false)
                throw new BadRequestException($"Account is not active with {request.UserName}.");

            var token = GenerateJWToken(user, _options);
            var refreshToken = GenerateRefreshToken(user, _dateTimeService, _options);

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var result = new AuthenticateDto { Token = token, RefreshToken = refreshToken.Token };
            return result;
        }


        public async Task ChangePasswordAsync(ChangePasswordCommand request)
        {
            var encPass = _encryptionService.HashPassword(request.CurrentPassword);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId && u.Password == encPass);
            if (user == null)
                throw new NotFoundException(nameof(user), request.UserId);

            user.Password = _encryptionService.HashPassword(request.CurrentPassword);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserRoleAsync(UpdateUserRoleCommand request)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(user), request.UserId);

            var (deleteRoles, addRoleIds) = ConsistencyUserRole(user.Roles, request.RoleIds);

            var addRoles = await _context.Roles.Where(r => addRoleIds.Contains(r.Id)).ToListAsync();
            if (addRoleIds.Count != addRoles.Count)
                throw new BadRequestException("Some RoleIds are Invalid.");

            user.Roles.AddRange(addRoles);
            user.Roles.RemoveRange(deleteRoles);

            await _context.SaveChangesAsync();
        }

        public async Task LogoutAsync(LogoutUserQuery request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal.Identity.GetUserId<long>();

            if (await _context.Users.AnyAsync(u => u.Id == userId) == false)
                throw new BadRequestException("Your token is invalid");

            var lastRefreshToken = await _context.RefreshTokens.OrderByDescending(o => o.Id).FirstOrDefaultAsync(u => u.UserId == userId);
            if (lastRefreshToken is null)
                throw new BadRequestException("First login to system.");

            lastRefreshToken.ExpirationDate = _dateTimeService.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<AuthenticateDto> RefreshTokenAsync(RefreshTokenQuery request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal.Identity.GetUserId<long>();

            var user = await _context.Users.Include(u => u.Roles).Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new BadRequestException("Your token is invalid");

            if (user.RefreshTokens.Any() == false)
                throw new BadRequestException("First login to system.");

            var lastRefreshToken = user.RefreshTokens.OrderByDescending(u => u.Id).FirstOrDefault();

            if (lastRefreshToken != null && lastRefreshToken.Token == request.RefreshToken || lastRefreshToken.ExpirationDate >= DateTime.Now)
            {
                var newAccessToken = GenerateJWToken(user, _options);
                string newRefreshToken = lastRefreshToken.Token;
                if ((lastRefreshToken.ExpirationDate - DateTime.Now).TotalMinutes < _options.CurrentValue.AccessTokenExpirationMinutes)
                {
                    var refreshToken = GenerateRefreshToken(user, _dateTimeService, _options);
                    await _context.RefreshTokens.AddAsync(refreshToken);
                    await _context.SaveChangesAsync();

                    newRefreshToken = refreshToken.Token;
                }

                return new AuthenticateDto { Token = newAccessToken, RefreshToken = newRefreshToken };
            }
            else
                throw new BadRequestException("Invalid Token.");
        }

        #region Privates

        private string GenerateJWToken(User user, IOptionsMonitor<BearerTokensConfigurationModel> options)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, options.CurrentValue.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Surname, user.LastName??""),
                new Claim(ClaimTypes.GivenName, user.FirstName??"")
            };


            foreach (Role role in user.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role.Title));

            JwtSecurityToken token = CreateToken(options, claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static JwtSecurityToken CreateToken(IOptionsMonitor<BearerTokensConfigurationModel> options, List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.CurrentValue.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(issuer: options.CurrentValue.Issuer, claims: claims, notBefore: now, expires: now.AddMinutes(options.CurrentValue.AccessTokenExpirationMinutes), signingCredentials: creds);
            return token;
        }

        private static RefreshToken GenerateRefreshToken(User user, IDateTimeService dateTimeService, IOptionsMonitor<BearerTokensConfigurationModel> options)
        {
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                User = user,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                CreatedDate = dateTimeService.Now,
                ExpirationDate = dateTimeService.Now.AddDays(options.CurrentValue.RefreshTokenExpirationDays)
            };

            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _options.CurrentValue.Issuer,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.CurrentValue.Key)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;

        }

        private (List<Role> deleteRoles, List<long> addRoleIds) ConsistencyUserRole(IList<Role> roles, List<long> requestRoleIds)
        {
            var deleteRoles = roles.Where(rr => requestRoleIds.All(r => r != rr.Id)).ToList();
            var addRoleIds = requestRoleIds.Where(rr => roles.All(r => r.Id != rr)).ToList();

            return (deleteRoles, addRoleIds);
        }

        #endregion
    }
}
