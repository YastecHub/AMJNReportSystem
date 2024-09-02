using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    /// <summary>
    /// Interface class that handles Report submission window methods
    /// </summary>
    public interface ISubmissionWindowService
    {
        /// <summary>
        /// Method for creating new report submission window,by Admin only
        /// </summary>
        Task<Result<bool>> CreateReportSubmissionWindow<T>(CreateSubmissionWindowRequest request);

        /// <summary>
        /// Method for updating submission window by admin only, the starting and ending date of a submission window
        /// can be updated only when the window is not yet active (i/e the start date has not been reached) otherwise
        /// we should only be able to update the ending date (which should also not be less than current date)
        /// </summary>
        Task<Result<bool>> UpdateReportSubmissionWindow<T>(Guid id, UpdateSubmissionWindowRequest request);

        /// <summary>
        /// Method to get all active submission window
        /// </summary>
        Task<Result<PaginatedResult<T>>> GetActiveSubmissionWindows<T>(PaginationFilter filter);

        /// <summary>
        /// Method to get a submission window
        /// </summary>
        Task<Result<SubmissionWindowDto>> GetSubmissionWindow(Guid Id);

        Task<IEnumerable<SubmissionWindowResponseModel>> GetSubmissionWindows(Guid? reportTypeId, int? month, int? year, string? status, bool? isLocked, DateTime? startDate, DateTime? endDate);
    }
}
