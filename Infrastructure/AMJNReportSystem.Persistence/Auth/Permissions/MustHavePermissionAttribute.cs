using Application.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace AMJNReportSystem.Persistence.Auth.Permissions
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string action, string resource) =>
            Policy = GXPermission.NameFor(action, resource);
    }
}