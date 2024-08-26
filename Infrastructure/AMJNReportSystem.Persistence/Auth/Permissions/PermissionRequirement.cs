using Microsoft.AspNetCore.Authorization;

namespace AMJNReportSystem.Persistence.Auth.Permissions
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}