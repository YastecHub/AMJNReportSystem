using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    public interface IGenerateReportService 
    {
        Task<BaseResponse<string>> GenerateJamaatReportSubmissionsAsync(Guid jamaatSubmissionId);
        Task<BaseResponse<PdfResponse>> ReportSubmissionsAsync(Guid jamaatSubmissionId);
    } 
}
