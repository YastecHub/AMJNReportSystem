using System.Collections.ObjectModel;

namespace AMJNReportSystem.Application.Authorization
{
    public static class GXRoles
    {
        public const string SuperAdmin = nameof(SuperAdmin);
        public const string Qaid = nameof(Qaid);
        public const string Zaim = nameof(Zaim);
        public const string Nazim = nameof(Nazim);

        public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
        {
            SuperAdmin,
            Qaid,
            Zaim,
            Nazim
        });

        public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
    }
}