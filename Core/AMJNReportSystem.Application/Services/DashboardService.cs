using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Interfaces;
using AMJNReportSystem.Application.Models.DTOs;

namespace AMJNReportSystem.Application.Services
{
    public class DashboardService : IDashboardService
    {

        private readonly IReportTypeRepository _reportTypeRepository;
        private readonly ICurrentUser _currentUser;

        public DashboardService(IReportTypeRepository reportTypeRepository, ICurrentUser currentUser)
        {

            _reportTypeRepository = reportTypeRepository;
            _currentUser = currentUser;
        }

        public async Task<DashboardCountDto> DashBoardCountAsync(int? month = null)
        {
            try
            {
                var jamaatId = _currentUser.GetJamaatId();
                var circuitId = _currentUser.GetCircuit();

                return await _reportTypeRepository.DashBoardDataAsync(jamaatId, circuitId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching dashboard counts.", ex);
            }
        }


    }
}
