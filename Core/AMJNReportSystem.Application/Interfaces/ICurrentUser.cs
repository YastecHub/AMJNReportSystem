using System.Security.Claims;

namespace AMJNReportSystem.Application.Interfaces
{
    public interface ICurrentUser
    {
        string? Name { get; }

        Guid GetUserId();
        int GetJamaatId();
        int GetCircuit();

        string? GetUserEmail();

        bool IsAuthenticated();

        bool IsInRole(string role);

        IEnumerable<Claim>? GetUserClaims();
    }
}