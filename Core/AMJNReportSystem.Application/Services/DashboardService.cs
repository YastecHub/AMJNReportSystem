using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Identity.Users;
using AMJNReportSystem.Application.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IReportSubmissionRepository _reportSubmissionRepository; 
        private readonly IReportTypeRepository _reportTypeRepository;
        private readonly IReportSectionRepository _reportSectionRepository; 
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserService _userService;
        public DashboardService(IReportSubmissionRepository reportSubmissionRepository,
                IReportTypeService reportTypeService, 
                IReportSectionRepository reportSectionRepository,
                IQuestionRepository questionRepository,
                IUserService _userService)
        {
            _reportSubmissionRepository = reportSubmissionRepository;
        }
        public Task<DashboardCountDto> DashBoardCount()
        {
            throw new NotImplementedException();
        }
    }
}
