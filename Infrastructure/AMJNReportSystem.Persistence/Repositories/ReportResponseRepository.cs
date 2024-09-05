using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Repositories;
using AMJNReportSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AMJNReportSystem.Infrastructure.Repositories
{
    public class ReportResponseRepository : IReportResponseRepository
    {
        private readonly ApplicationContext _context;

        public ReportResponseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReportResponse>> GetAllReportResponseAsync()
        {
            return await _context.ReportResponses
                .Include(rr => rr.Question)
                .Include(rr => rr.QuestionOption)
                .ToListAsync();
        }

        public async Task<ReportResponse?> GetReportResponseByIdAsync(Guid responseId)
        {
            return await _context.ReportResponses
                .Include(rr => rr.Question)
                .Include(rr => rr.QuestionOption)
                .FirstOrDefaultAsync(rr => rr.Id == responseId);
        }

        public async Task<ReportResponse> AddReportResponseAsync(ReportResponse reportResponse)
        {
            _context.ReportResponses.Add(reportResponse);
            await _context.SaveChangesAsync();
            return reportResponse;
        }

        public async Task<ReportResponse> UpdateReportResponseAsync(ReportResponse reportResponse)
        {
            _context.ReportResponses.Update(reportResponse);
            await _context.SaveChangesAsync();
            return reportResponse;
        }

        public async Task<bool> DeleteReportResponseAsync(Guid responseId)
        {
            var reportResponse = await _context.ReportResponses.FindAsync(responseId);
            if (reportResponse == null) return false;

            _context.ReportResponses.Remove(reportResponse);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> QuestionExistsAsync(Guid questionId)
        {
            return await _context.Questions.AnyAsync(q => q.Id == questionId);
        }

        public async Task<bool> QuestionOptionExistsAsync(Guid questionOptionId)
        {
            return await _context.QuestionOptions.AnyAsync(qo => qo.Id == questionOptionId);
        }
    }
}
