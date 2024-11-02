using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    public interface IGenerateReportService 
    {
        // Task<BaseResponse<List<ReportSubmissionResponseDto>>> GenerateJamaatReportSubmissionsAsync(int jamaatId, int month);

        Task<BaseResponse<string>> GenerateJamaatReportSubmissionsAsync(int jamaatId, int month);
    } 
}
