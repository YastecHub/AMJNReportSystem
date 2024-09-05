using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Domain.Repositories
{
    public interface IReportResponseRepository
    {
        Task<IEnumerable<ReportResponse>> GetAllReportResponseAsync();
        Task<ReportResponse?> GetReportResponseByIdAsync(Guid responseId);
        Task<ReportResponse> AddReportResponseAsync(ReportResponse reportResponse);
        Task<ReportResponse> UpdateReportResponseAsync(ReportResponse reportResponse);
        Task<bool> DeleteReportResponseAsync(Guid responseId);
        Task<bool> QuestionExistsAsync(Guid questionId);
        Task<bool> QuestionOptionExistsAsync(Guid questionOptionId);
    }
}
