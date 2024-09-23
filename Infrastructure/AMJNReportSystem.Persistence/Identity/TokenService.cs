using AMJNReportSystem.Application;
using AMJNReportSystem.Application.Exceptions;
using AMJNReportSystem.Application.Identity.Tokens;
using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Persistence.Auth;
using AMJNReportSystem.Persistence.Auth.Jwt;
using AMJNReportSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AMJNReportSystem.Persistence.Identity
{
    public class TokenService : ITokenService
    {

        private readonly IStringLocalizer _t;
        private readonly IGatewayHandler _gatewayHandler;
        private readonly ApplicationContext _dbContext;
        private readonly SecuritySettings _securitySettings;
        private readonly JwtSettings _jwtSettings;

        public TokenService(
            IOptions<JwtSettings> jwtSettings,
            IStringLocalizer<TokenService> localizer,
            IOptions<SecuritySettings> securitySettings, IGatewayHandler gatewayHandler, ApplicationContext dbContext)
        {
            _t = localizer;
            _gatewayHandler = gatewayHandler;
            _dbContext = dbContext;
            _jwtSettings = jwtSettings.Value;
            _securitySettings = securitySettings.Value;
        }

        public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken)
        {
            var login = await _gatewayHandler.GenerateToken(request);

            if (login == null)
            {
                throw new UnauthorizedException(_t["User Not Active. Please contact the administrator."]);
            }

            var user = await _gatewayHandler.GetMemberByChandaNoAsync(int.Parse(login.Data.UserName));

            if (user == null)
            {

                throw new UnauthorizedException(_t[$"Authentication Failed."]);
            }
            user.Roles = string.Join(", ", login.Data.Roles);
            return await GenerateTokensAndUpdateUser(user, ipAddress);
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
        {
            var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
            string? userEmail = userPrincipal.GetUserId();

            int chandaNo = !string.IsNullOrEmpty(userEmail) ? int.Parse(userEmail) : 0;
            var user = await _gatewayHandler.GetMemberByChandaNoAsync(chandaNo);
            if (user is null)
            {
                throw new UnauthorizedException(_t["Authentication Failed."]);
            }

            var userRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.ChandaNo == userEmail);

            if (userRefreshToken != null)
            {
                if (userRefreshToken.Token != request.RefreshToken || userRefreshToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    throw new UnauthorizedException(_t["Invalid Refresh Token."]);
                }
            }
            return await GenerateTokensAndUpdateUser(user, ipAddress);
        }

        private async Task<TokenResponse> GenerateTokensAndUpdateUser(User user, string ipAddress)
        {
            string token = GenerateJwt(user, ipAddress);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

            var userRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.ChandaNo == user.ChandaNo);

            if (userRefreshToken != null)
            {
                userRefreshToken.Token = token;
                userRefreshToken.RefreshTokenExpiryTime = refreshTokenExpiryTime;

                _dbContext.RefreshTokens.Update(userRefreshToken);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                await _dbContext.RefreshTokens.AddAsync(new RefreshToken
                {
                    ChandaNo = user.ChandaNo,
                    RefreshTokenExpiryTime = refreshTokenExpiryTime,
                    Token = token,
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                });
                await _dbContext.SaveChangesAsync();
            }
            return new TokenResponse(token, refreshToken, refreshTokenExpiryTime);
        }

        private string GenerateJwt(User user, string ipAddress) =>
            GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

        private IEnumerable<Claim> GetClaims(User user, string ipAddress) =>
            new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.ChandaNo),
                new(ClaimTypes.Email, user.Email!),
                new("FullName", $"{user.FirstName} {user.MiddleName} {user.Surname}"),
                new(ClaimTypes.Name, user.FirstName ?? string.Empty),
                new(ClaimTypes.Surname, user.Surname ?? string.Empty),
                new(ClaimTypes.MobilePhone, user.PhoneNo ?? string.Empty),
                new(ClaimTypes.Role, user.Roles ?? string.Empty ),
                new("JamaatId", user.JamaatId.ToString() ?? string.Empty),
                new("CircuitId", user.CircuitId.ToString() ?? string.Empty),

            };

        private static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnauthorizedException(_t["Invalid Token."]);
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
    }
}