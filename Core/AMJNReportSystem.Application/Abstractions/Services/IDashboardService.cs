using AMJNReportSystem.Application.Models.DTOs;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    public interface IDashboardService
    {
        Task<DashboardCountDto> DashBoardCount(int? month = null);
    }
}
