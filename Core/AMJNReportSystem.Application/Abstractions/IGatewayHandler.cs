using AMJNReportSystem.Application.Identity.Tokens;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application
{
    public interface IGatewayHandler
    {
        Task<PaginatedResult<Muqam>> GeMuqamatAsync(PaginationFilter filter);
        Task<PaginatedResult<Dila>> GetDilaatAsync(PaginationFilter filter);
        Task<PaginatedResult<Zone>> GetZonesAsync(PaginationFilter filter);
        Task<string[]> GetMemberRoleAsync(int chandaNo); 
        Task<User> GetMemberByChandaNoAsync(int chandaNo); 
        Task<MemberApiLoginResponse> GenerateToken(TokenRequest request);
        Task<List<Muqam>?> GetListOfMuqamAsync();
    }
}
