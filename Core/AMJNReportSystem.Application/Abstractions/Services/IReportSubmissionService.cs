using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.RequestModels.Reports;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    /// <summary>
    /// Interface class that handles Report submission methods
    /// </summary>
    public interface IReportSubmissionService
    {
        Task<BaseResponse<ReportSubmissionResponseDto>> GetReportTypeSubmissionByIdAsync(Guid reportTypeSubmissionId);
        Task<BaseResponse<bool>> CreateReportTypeSubmissionAsync(CreateReportSubmissionRequest request);
        Task<BaseResponse<ReportSubmissionDto>> UpdateReportSubmission(Guid id, UpdateReportSubmission request);
        Task<BaseResponse<PaginatedResult<ReportSubmissionDto>>> GetAllReportTypeSubmissionsAsync(PaginationFilter filter);
        Task<Result<bool>> DeleteReportSubmission(Guid reportSubmissionId);
    }
}

