using AMJNReportSystem.Persistence.Auth;
using AMJNReportSystem.Persistence.Auth.Jwt;
using Application.Authorization;
using Application.Exceptions;
using Application.Identity.Tokens;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer _t;
        private readonly SecuritySettings _securitySettings;
        private readonly JwtSettings _jwtSettings;

        public TokenService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            IStringLocalizer<TokenService> localizer,
            IOptions<SecuritySettings> securitySettings)
        {
            _userManager = userManager;
            _t = localizer;
            _jwtSettings = jwtSettings.Value;
            _securitySettings = securitySettings.Value;
        }

        public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.Trim().Normalize());

            if (!user.IsActive)
            {
                throw new UnauthorizedException(_t["User Not Active. Please contact the administrator."]);
            }

            bool isLockedOut = await _userManager.IsLockedOutAsync(user);

            if (isLockedOut)
            {
                DateTimeOffset? lockoutEndDate = await _userManager.GetLockoutEndDateAsync(user);
                TimeSpan? remainingTime = lockoutEndDate - DateTimeOffset.UtcNow;

                throw new UnauthorizedException(_t[$"Authentication Failed. Account locked, Please try again after {remainingTime?.TotalMinutes} minutes."]);
            }

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                //user.AccessFailedCount++;
                await _userManager.AccessFailedAsync(user);
                if (user.AccessFailedCount == 4)
                {
                    //user.IsActive = false;
                    // Set the lockout end date to 5 minutes from now
                    DateTimeOffset lockoutEndDate = DateTimeOffset.UtcNow.AddMinutes(5);
                    await _userManager.SetLockoutEndDateAsync(user, lockoutEndDate);

                    TimeSpan? remainingTime = lockoutEndDate - DateTimeOffset.UtcNow;
                    await _userManager.UpdateAsync(user);
                    throw new UnauthorizedException(_t[$"Authentication Failed. Account Locked after 4 failed attempts. Please try again after {remainingTime?.TotalMinutes} minutes."]);
                }
                await _userManager.UpdateAsync(user);
                throw new UnauthorizedException(_t["Authentication Failed."]);
            }

            if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
            {
                throw new UnauthorizedException(_t["E-Mail not confirmed."]);
            }

            //user.AccessFailedCount = 0;
            await _userManager.ResetAccessFailedCountAsync(user);
            // 
            return await GenerateTokensAndUpdateUser(user, ipAddress);
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
        {
            var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
            string? userEmail = userPrincipal.GetEmail();
            var user = await _userManager.FindByEmailAsync(userEmail!);
            if (user is null)
            {
                throw new UnauthorizedException(_t["Authentication Failed."]);
            }

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException(_t["Invalid Refresh Token."]);
            }

            return await GenerateTokensAndUpdateUser(user, ipAddress);
        }

        private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress)
        {
            string token = GenerateJwt(user, ipAddress);

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

            await _userManager.UpdateAsync(user);

            return new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
        }

        private string GenerateJwt(ApplicationUser user, string ipAddress) =>
            GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

        private IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress) =>
            new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email!),
                new(GXClaims.Fullname, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Name, user.FirstName ?? string.Empty),
                new(ClaimTypes.Surname, user.LastName ?? string.Empty),
                new(GXClaims.IpAddress, ipAddress),
                new(GXClaims.ImageUrl, user.ImageUrl ?? string.Empty),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
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