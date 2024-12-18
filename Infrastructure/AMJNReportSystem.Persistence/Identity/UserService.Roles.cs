﻿using AMJNReportSystem.Application.Authorization;
using AMJNReportSystem.Application.Exceptions;
using AMJNReportSystem.Application.Identity.Users;
using Microsoft.EntityFrameworkCore;

namespace AMJNReportSystem.Persistence.Identity
{
    public partial class UserService
    {
        public async Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken)
        {
            var userRoles = new List<UserRoleDto>();

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new NotFoundException("User Not Found.");
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken);
            if (roles is null) throw new NotFoundException("Roles Not Found.");
            foreach (var role in roles)
            {
                userRoles.Add(new UserRoleDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Description = role.Description,
                    Enabled = await _userManager.IsInRoleAsync(user, role.Name!)
                });
            }

            return userRoles;
        }

        public async Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException(_t["User Not Found."]);

            // Check if the user is an admin for which the admin role is getting disabled
            if (await _userManager.IsInRoleAsync(user, GXRoles.SuperAdmin)
                && request.UserRoles.Any(a => !a.Enabled && a.RoleName == GXRoles.SuperAdmin))
            {
                // Get count of users in Admin Role
                int adminCount = (await _userManager.GetUsersInRoleAsync(GXRoles.SuperAdmin)).Count;

                // Check if user is not Root Tenant Admin
                // Edge Case : there are chances for other tenants to have users with the same email as that of Root Tenant Admin. Probably can add a check while User Registration

            }

            foreach (var userRole in request.UserRoles)
            {
                // Check if Role Exists
                if (await _roleManager.FindByNameAsync(userRole.RoleName!) is not null)
                {
                    if (userRole.Enabled)
                    {
                        if (!await _userManager.IsInRoleAsync(user, userRole.RoleName!))
                        {
                            await _userManager.AddToRoleAsync(user, userRole.RoleName!);
                        }
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, userRole.RoleName!);
                    }
                }
            }

            return _t["User Roles Updated Successfully."];
        }
    }
}