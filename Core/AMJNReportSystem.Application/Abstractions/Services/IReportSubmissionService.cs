using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    /// <summary>
    /// Interface class that handles Report submission methods
    /// </summary>
    public interface IReportSubmissionService
    {
        /*/// <summary>
        /// Method for creating new report submission,by Admin only
        /// </summary>
        Task<Result<T>> CreateReportTypeSubmission<T>(CreateReportSubmissionRequest request);

        /// <summary>
        /// Method that get a particular report type submission, accepting the reportType submission Id as parameter
        /// </summary>
        Task<Result<T>> GetReportTypeSubmission<T>(Guid reportTypeSubmissionId);

        /// <summary>
        /// Method that get all report types submission by year
        /// </summary>
        Task<Result<PaginatedResult<T>>> GetReportTypeSubmissions<T>(PaginationFilter filter);
*/
        Task<Result<bool>> CreateReportTypeSubmissionAsync(CreateReportSubmissionRequest request);
        Task<Result<ReportSubmissionResponseModel>> GetReportTypeSubmissionAsync(Guid reportTypeSubmissionId);
        Task<Result<PaginatedResult<ReportSubmissionResponseModel>>> GetReportTypeSubmissionsAsync(PaginationFilter filter);
    }
}
