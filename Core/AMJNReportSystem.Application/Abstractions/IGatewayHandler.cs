using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Wrapper;
using Domain.Entities;

namespace AMJNReportSystem.Application
{
    public interface IGatewayHandler
    {
        Task<PaginatedResult<Muqam>> GeMuqamatAsync(PaginationFilter filter);
        Task<PaginatedResult<Dila>> GetDilaatAsync(PaginationFilter filter);
        Task<PaginatedResult<Zone>> GetZonesAsync(PaginationFilter filter);
    }
}
