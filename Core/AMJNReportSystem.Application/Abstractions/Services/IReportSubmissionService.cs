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
        Task<BaseResponse<SubmittedReportDto>> GetSectionReportSubmissionAsync(Guid reportTypeSubmissionId, Guid reportSectionId);
        Task<BaseResponse<bool>> CreateAndUpdateReportSubmissionAsync(CreateReportSubmissionRequest request);
        Task<BaseResponse<ReportSubmissionDto>> UpdateReportSubmission(Guid id, UpdateReportSubmission request);
        Task<BaseResponse<PaginatedResult<ReportSubmissionResponseDto>>> GetAllReportTypeSubmissionsAsync(PaginationFilter filter);
        Task<Result<bool>> DeleteReportSubmission(Guid reportSubmissionId);
        Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetReportSubmissionsByReportTypeAsync(Guid reportTypeId);
        Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetReportSubmissionsByCircuitIdAsync();
        Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetReportSubmissionsByJamaatIdAsync();
        Task<BaseResponse<List<ReportSubmissionResponseDto>>> GetAllReportTypeSubmissionsAsync();
        Task<BaseResponse<List<JamaatReport>>> GetJamaatReportsBySubmissionWindowIdAsync(Guid submissionWindowId);
        Task<BaseResponse<bool>> ConfirmReportSectionHasBeenSubmittedAsync(Guid reportTypeSubmissionId, Guid reportSectionId);
        Task<BaseResponse<AmjnReportByRole>> GetJamaatReportByRoleAsync(Guid submissionWindowId);
    }
}

