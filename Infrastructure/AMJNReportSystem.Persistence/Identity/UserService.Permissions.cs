﻿using AMJNReportSystem.Application.Exceptions;
using AMJNReportSystem.Application.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AMJNReportSystem.Persistence.Identity
{
    public partial class UserService
    {
        public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            _ = user ?? throw new UnauthorizedException("Authentication Failed.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var permissions = new List<string>();
            foreach (var role in await _roleManager.Roles
                .Where(r => userRoles.Contains(r.Name!))
                .ToListAsync(cancellationToken))
            {
                permissions.AddRange(await _db.RoleClaims
                    .Where(rc => rc.RoleId == role.Id && rc.ClaimType == GXClaims.Permission)
                    .Select(rc => rc.ClaimValue!)
                    .ToListAsync(cancellationToken));
            }

            return permissions.Distinct().ToList();
        }

        public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken)
        {
            /*var permissions = await _cache.GetOrSetAsync(
                _cacheKeys.GetCacheKey(GXClaims.Permission, userId),
                () => GetPermissionsAsync(userId, cancellationToken),
                cancellationToken: cancellationToken);*/
            var permissions = await GetPermissionsAsync(userId, cancellationToken);

            return permissions?.Contains(permission) ?? false;
        }

        /*   public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken) =>
               _cache.RemoveAsync(_cacheKeys.GetCacheKey(FSHClaims.Permission, userId), cancellationToken);*/

        public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}