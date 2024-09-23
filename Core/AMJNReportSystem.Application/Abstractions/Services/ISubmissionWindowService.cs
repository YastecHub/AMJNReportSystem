using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;    

namespace AMJNReportSystem.Application.Abstractions.Services
{ 
    public interface ISubmissionWindowService
    {
        Task<Result<bool>> CreateReportSubmissionWindow<T>(CreateSubmissionWindowRequest request);

        Task<Result<bool>> UpdateReportSubmissionWindow<T>(Guid id, UpdateSubmissionWindowRequest request);
        Task<Result<bool>> DeleteSubmissionWindow(Guid subWindowId);

        Task<Result<SubmissionWindowDto>> GetActiveSubmissionWindows();

        Task<Result<SubmissionWindowDto>> GetSubmissionWindow(Guid Id);

        Task<Result<IList<SubmissionWindowDto>>> GetSubmissionWindows();
    }
}
