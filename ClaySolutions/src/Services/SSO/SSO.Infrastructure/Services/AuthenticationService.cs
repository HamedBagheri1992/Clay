using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Exceptions;
using SSO.Application.Common.Settings;
using SSO.Application.Contracts.Infrastructure;
using SSO.Application.Contracts.Persistence;
using SSO.Application.Features.Account.Commands.ChangePassword;
using SSO.Application.Features.Account.Commands.UpdateUserRole;
using SSO.Application.Features.Account.Queries.Authenticate;
using SSO.Application.Features.Account.Queries.LogoutUser;
using SSO.Application.Features.Account.Queries.RefreshToken;
using SSO.Domain.Entities;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Linq;
using SharedKernel.Extensions;

namespace SSO.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly IOptionsMonitor<BearerTokensConfigurationModel> _options;

        public AuthenticationService(IEncryptionService encryptionService, IUserRepository userRepository, IRoleRepository roleRepository,
            IRefreshTokenRepository refreshTokenRepository, IDateTimeService dateTimeService, IOptionsMonitor<BearerTokensConfigurationModel> options)
        {
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _dateTimeService = dateTimeService;
            _options = options;
        }

        public async Task<AuthenticateDto> AuthenticateAsync(AuthenticateQuery request)
        {
            string encPass = _encryptionService.HashPassword(request.Password);
            var user = await _userRepository.GetUserWithRolesAsync(request.UserName, encPass);
            if (user == null)
                throw new NotFoundException($"No Accounts Registered", request.UserName);

            if (user.IsActive == false)
                throw new BadRequestException($"Account is not active with {request.UserName}.");

            var token = GenerateJWToken(user, _options);
            var refreshToken = GenerateRefreshToken(user, _dateTimeService, _options);

            await _refreshTokenRepository.AddAsync(refreshToken);

            var result = new AuthenticateDto { Token = token, RefreshToken = refreshToken.Token };
            return result;
        }

        public async Task ChangePasswordAsync(ChangePasswordCommand request)
        {
            var encPass = _encryptionService.HashPassword(request.CurrentPassword);
            var user = await _userRepository.GetUserByPasswordAsync(request.UserId, encPass);
            if (user == null)
                throw new NotFoundException(nameof(user), request.UserId);

            user.Password = _encryptionService.HashPassword(request.NewPassword);
            await _userRepository.UpdateAsync(user);
        }

        public async Task UpdateUserRoleAsync(UpdateUserRoleCommand request)
        {
            var user = await _userRepository.GetUserWithRolesAsync(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(user), request.UserId);

            var (deleteRoles, addRoleIds) = ConsistencyUserRole(user.Roles, request.RoleIds);

            if (addRoleIds.Count > 0)
            {
                var addRoles = await _roleRepository.GetByRoleIdsAsync(addRoleIds);
                if (addRoleIds.Count != addRoles.Count)
                    throw new BadRequestException("Some RoleIds are Invalid.");
                user.Roles.AddRange(addRoles);
            }

            if (deleteRoles.Count > 0)
                user.Roles.RemoveRange(deleteRoles);

            await _userRepository.UpdateAsync(user);
        }

        public async Task LogoutAsync(LogoutUserQuery request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal.Identity.GetUserId<long>();

            if (await _userRepository.GetAsync(request.UserId) == null)
                throw new BadRequestException("Your token is invalid");

            var lastRefreshToken = await _refreshTokenRepository.GetLatestOneAsync(userId);
            if (lastRefreshToken is null)
                throw new BadRequestException("First login to system.");

            lastRefreshToken.ExpirationDate = _dateTimeService.Now;
            await _refreshTokenRepository.UpdateAsync(lastRefreshToken);
        }

        public async Task<AuthenticateDto> RefreshTokenAsync(RefreshTokenQuery request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal.Identity.GetUserId<long>();

            var user = await _userRepository.GetWithRoleAndRefreshTokensAsync(userId);
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
                    await _refreshTokenRepository.AddAsync(refreshToken);

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
