using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Mapster;

namespace AMJNReportSystem.Application.Services
{
    public class ReportSubmissionService : IReportSubmissionService
    {
        private readonly IReportSubmissionRepository _reportSubmissionRepository;
        private readonly IReportTypeRepository _reportTypeRepository;

        public ReportSubmissionService(IReportSubmissionRepository reportSubmission, IReportTypeRepository reportTypeRepository)
        {
            _reportSubmissionRepository = reportSubmission;
            _reportTypeRepository = reportTypeRepository;
        }

        public async Task<Result<bool>> CreateReportTypeSubmissionAsync(CreateReportSubmissionRequest request)
        {
            if (request is null) return await Result<bool>.FailAsync("report submission not found.");
            var reportType = await _reportTypeRepository.GetReportTypeById(request.ReportTypeId);
            var reportSubmissionName = $"{reportType?.Title}_{request.Year}_{request.Month}";
            var reportSubmissionChecker = await _reportSubmissionRepository.Exist(reportSubmissionName);
            if (reportSubmissionChecker) return await Result<bool>.FailAsync("Submission date already exit");
            var submission = request.Adapt<ReportSubmission>();
            submission.ReportType.Title = reportSubmissionName;
            await _reportSubmissionRepository.AddReportSubmissionAsync(submission);
            return await Result<bool>.SuccessAsync("Report submission successfully added");
        }
        public async Task<Result<ReportSubmissionResponseModel>> GetReportTypeSubmissionAsync(Guid reportTypeSubmissionId)
        {
            var reportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionAsync(x => x.Id == reportTypeSubmissionId);
            if (reportSubmission is null) return await Result<ReportSubmissionResponseModel>.FailAsync("Report submission Type not found");
            var reportSubmissionResponse = reportSubmission.Adapt<ReportSubmissionResponseModel>();
            return await Result<ReportSubmissionResponseModel>.SuccessAsync(reportSubmissionResponse, "Report submission Successfully Retrieved");
        }

        public async Task<Result<PaginatedResult<ReportSubmissionResponseModel>>> GetReportTypeSubmissionsAsync(PaginationFilter filter)
        {
            var reportSubmission = await _reportSubmissionRepository.GetReportTypeSubmissionsAsync(filter);
            return await Result<PaginatedResult<ReportSubmissionResponseModel>>.SuccessAsync(reportSubmission, $"{reportSubmission.TotalCount} report type submission retrieved successfully..");
        }

    }
}
