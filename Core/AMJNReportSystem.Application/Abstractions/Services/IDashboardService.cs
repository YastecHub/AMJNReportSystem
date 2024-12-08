using AMJNReportSystem.Application.Models.DTOs;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    public interface IDashboardService
    {
        Task<DashboardCountDto> DashBoardCountAsync(int? month = null);
    }
}
