using AMJNReportSystem.Application.Identity.Tokens;
using AMJNReportSystem.Application.Interfaces;
using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Identity.Users
{
    public interface IUserService : ITransientService
    {
        Task<string[]> GetMemberRoleAsync(int chandaNo);
        Task<User> GetMemberByChandaNoAsync(int chandaNo);
        Task<MemberApiLoginResponse> GenerateToken(TokenRequest request);
    }
}