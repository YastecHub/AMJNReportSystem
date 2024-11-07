using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;

namespace AMJNReportSystem.Application.Services
{
    public class DashboardService : IDashboardService
    {

        private readonly IReportTypeRepository _reportTypeRepository;
        public DashboardService(IReportTypeRepository reportTypeRepository)
        {

            _reportTypeRepository = reportTypeRepository;
        }

        public async Task<DashboardCountDto> DashBoardCount(int? month = null) 
            => await _reportTypeRepository.DashBoardDataAsync();
    }
}
