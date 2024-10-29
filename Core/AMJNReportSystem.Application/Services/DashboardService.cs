using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;

namespace AMJNReportSystem.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IReportSubmissionRepository _reportSubmissionRepository;
        private readonly IReportTypeRepository _reportTypeRepository;
        private readonly IReportSectionRepository _reportSectionRepository;
        private readonly IQuestionRepository _questionRepository;
        public DashboardService(IReportSubmissionRepository reportSubmissionRepository,
                IReportTypeRepository reportTypeRepository,
                IReportSectionRepository reportSectionRepository,
                IQuestionRepository questionRepository)
        {
            _reportSubmissionRepository = reportSubmissionRepository;
            _reportTypeRepository = reportTypeRepository;
            _reportSectionRepository = reportSectionRepository;
            _questionRepository = questionRepository;


        }
        public DashboardCountDto DashBoardCount()
        {
            var reportTypeCount = _reportTypeRepository.GetAllReportType();
            var reportSectionCount = _reportSectionRepository.GetAllReportSection();
            var reportSubmissionCount = _reportSubmissionRepository.GetAllReportSubmission();
            var questionCount = _questionRepository.GetAllQuestion();



            var data = new DashboardCountDto();

            data.ReportTypeCounts = reportTypeCount.Count();
            data.ReportSectionCounts = reportSectionCount.Count();
            data.ReportSubmittedByJamaatCounts = reportSubmissionCount.Count();
            data.ReportSubmittedByCircuitCounts = reportSubmissionCount.Count();
            data.QuestionCounts = questionCount.Count();
            return data;

        }
    }
}
