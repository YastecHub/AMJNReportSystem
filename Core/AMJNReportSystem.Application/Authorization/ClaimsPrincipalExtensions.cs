namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetEmail(this ClaimsPrincipal principal)
            => principal.FindFirstValue(ClaimTypes.Email);

        public static string? GetFullName(this ClaimsPrincipal principal)
            => principal?.FindFirst("Fullname")?.Value;

        public static string? GetFirstName(this ClaimsPrincipal principal)
            => principal?.FindFirst(ClaimTypes.Name)?.Value;

        public static string? GetSurname(this ClaimsPrincipal principal)
            => principal?.FindFirst(ClaimTypes.Surname)?.Value;

        public static string? GetPhoneNumber(this ClaimsPrincipal principal)
            => principal.FindFirstValue(ClaimTypes.MobilePhone);

        public static string? GetUserId(this ClaimsPrincipal principal)
           => principal.FindFirstValue(ClaimTypes.NameIdentifier);

        public static string? GetImageUrl(this ClaimsPrincipal principal)
          => principal.FindFirstValue("ImageUrl");

        public static DateTimeOffset GetExpiration(this ClaimsPrincipal principal) =>
            DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(
                principal.FindFirstValue("Expiration")));

        public static List<string>? GetUserLoggedInRoles(this ClaimsPrincipal principal)
        {
            var role = principal.FindFirstValue(ClaimTypes.Role);

            if (!string.IsNullOrWhiteSpace(role))
            {
                return role.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(role => role.Trim())
                               .ToList();
            }
            return new List<string>();
        }

        public static int? GetCircuitId(this ClaimsPrincipal principal)
        {
            var circuit = principal.FindFirstValue("CircuitId");
            if (!string.IsNullOrWhiteSpace(circuit))
            {
                return int.Parse(circuit);
            }
            return null;
        }

        public static int? GetJamaatId(this ClaimsPrincipal principal)
        {
            var jamaat = principal.FindFirstValue("JamaatId");
            if (!string.IsNullOrWhiteSpace(jamaat))
            {
                return int.Parse(jamaat);
            }
            return null;
        }
    }
}