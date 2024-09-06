using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Domain.Repositories
{
    public interface IReportResponseService
    {
        /// <summary>
        /// Gets all report responses.
        /// </summary>
        Task<IEnumerable<ReportResponseDto>> GetAllReportResponsesAsync();
        /// <summary>
        /// Gets a specific report response by its identifier.
        /// </summary>
        
        Task<ReportResponseDto?> GetReportResponseByIdAsync(Guid responseId);

        /// <summary>
        /// Adds a new report response to the data source.
        /// </summary>

        Task<Result<ReportResponseDto>> CreateReportResponseAsync(CreateReportResponseRequest responseDto);

        /// <summary>
        /// Updates an existing report response in the data source.
        /// </summary>

        Task<Result<ReportResponseDto>> UpdateReportResponseAsync(Guid reportResponseId, UpdateReportResponseRequest responseDto);

        /// <summary>
        /// Deletes a report response by its identifier.
        /// </summary>
        /// <param name="responseId">The unique identifier of the report response to delete.</param>
        Task<bool> DeleteReportResponseAsync(Guid responseId);
    }
}
