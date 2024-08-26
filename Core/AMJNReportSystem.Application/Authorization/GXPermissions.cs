using System.Collections.ObjectModel;

namespace AMJNReportSystem.Application.Authorization
{
    public static class GXAction
    {
        public const string View = nameof(View);
        public const string Search = nameof(Search);
        public const string Create = nameof(Create);
        public const string Update = nameof(Update);
        public const string Delete = nameof(Delete);
        public const string Export = nameof(Export);
        public const string Generate = nameof(Generate);
        public const string Clean = nameof(Clean);
        public const string UpgradeSubscription = nameof(UpgradeSubscription);
    }

    public static class GXResource
    {
        public const string Tenants = nameof(Tenants);
        public const string Dashboard = nameof(Dashboard);
        public const string Hangfire = nameof(Hangfire);
        public const string Users = nameof(Users);
        public const string UserRoles = nameof(UserRoles);
        public const string Roles = nameof(Roles);
        public const string RoleClaims = nameof(RoleClaims);
        public const string Clients = nameof(Clients);
        public const string Coachs = nameof(Coachs);
    }

    public static class GXPermissions
    {
        private static readonly GXPermission[] _all = new GXPermission[]
        {
            new("View Dashboard", GXAction.View, GXResource.Dashboard),
            new("View Hangfire", GXAction.View, GXResource.Hangfire),
            new("View Users", GXAction.View, GXResource.Users),
            new("Search Users", GXAction.Search, GXResource.Users),
            new("Create Users", GXAction.Create, GXResource.Users),
            new("Update Users", GXAction.Update, GXResource.Users),
            new("Delete Users", GXAction.Delete, GXResource.Users),
            new("Export Users", GXAction.Export, GXResource.Users),
            new("View UserRoles", GXAction.View, GXResource.UserRoles),
            new("Update UserRoles", GXAction.Update, GXResource.UserRoles),
            new("View Roles", GXAction.View, GXResource.Roles),
            new("Create Roles", GXAction.Create, GXResource.Roles),
            new("Update Roles", GXAction.Update, GXResource.Roles),
            new("Delete Roles", GXAction.Delete, GXResource.Roles),
            new("View RoleClaims", GXAction.View, GXResource.RoleClaims),
            new("Update RoleClaims", GXAction.Update, GXResource.RoleClaims),
            new("View Clients", GXAction.View, GXResource.Clients),
            new("Search Clients", GXAction.Search, GXResource.Clients),
            new("Update Clients", GXAction.Update, GXResource.Clients),
            new("Delete Clients", GXAction.Delete, GXResource.Clients),
            new("Upgrade Tenant Subscription", GXAction.UpgradeSubscription, GXResource.Tenants, IsRoot: true)
        };

        public static IReadOnlyList<GXPermission> All { get; } = new ReadOnlyCollection<GXPermission>(_all);
        public static IReadOnlyList<GXPermission> Root { get; } = new ReadOnlyCollection<GXPermission>(_all.Where(p => p.IsRoot).ToArray());
        public static IReadOnlyList<GXPermission> Admin { get; } = new ReadOnlyCollection<GXPermission>(_all.Where(p => !p.IsRoot).ToArray());
        public static IReadOnlyList<GXPermission> Basic { get; } = new ReadOnlyCollection<GXPermission>(_all.Where(p => p.IsBasic).ToArray());
    }

    public record GXPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
    {
        public string Name => NameFor(Action, Resource);
        public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
    }
}
