using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    public interface IGenerateReportService 
    {
        Task<BaseResponse<string>> GenerateJamaatReportSubmissionsAsync(Guid jamaatSubmissionId);
    } 
}
