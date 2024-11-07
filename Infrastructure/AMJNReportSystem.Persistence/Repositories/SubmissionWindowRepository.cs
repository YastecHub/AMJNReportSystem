using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Org.BouncyCastle.Ocsp;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class SubmissionWindowRepository : ISubmissionWindowRepository
    {
        private readonly ApplicationContext _context;
        public SubmissionWindowRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<bool> AddSubmissionWindow(SubmissionWindow submissionWindow)
        {
            var submission = await _context.AddAsync(submissionWindow);
            _context.SaveChanges();
            return true;

        }

        public async Task<IList<SubmissionWindow>> GetAllSubmissionWindowsAsync(Expression<Func<SubmissionWindow, bool>> expression)
        {
            var submissionWindow = await _context.SubmissionWindows
               .Include(x => x.ReportType)
               .Where(expression)
               .ToListAsync();
            return submissionWindow;
        }
        public async Task<SubmissionWindow> GetActiveSubmissionWindows(Guid id)
        {
            var submissionWindow = await _context.SubmissionWindows
                .Include(x => x.ReportType)
                .SingleOrDefaultAsync(q => q.Id == id);
            return submissionWindow;
        }



        public async Task<SubmissionWindow> GetSubmissionWindowsById(Guid id)
        {
            var submissionWindow = await _context.SubmissionWindows
                .Include(x => x.ReportType)
                .SingleOrDefaultAsync(q => q.Id == id);
            return submissionWindow;
        }

        public async Task<bool> GetReportTypeExist(Guid reportTypeId)
        {
            var submissionWindow = await _context.SubmissionWindows
                .Include(x => x.ReportType)
                .SingleOrDefaultAsync(q => q.ReportTypeId == reportTypeId);

            return submissionWindow != null;
        }


        public async Task<bool> UpdateSubmissionWindow(SubmissionWindow submissionWindow)
        {
            var submission = _context.Update(submissionWindow);
            _context.SaveChanges();
            return true;
        }


        public async Task<ReportSubmission?> CheckIfReportHasBeenSubmittedByJammatPresident(Guid submissionWindowId, int JamaatId)
        {
            var submissionWindow = await _context.ReportSubmissions
                .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .FirstOrDefaultAsync(q => q.SubmissionWindowId == submissionWindowId && q.JamaatId == JamaatId);
            return submissionWindow;
        }

        public async Task<ReportSubmission?> CheckIfReportSectionHasBeenSubmitted(Guid submissionWindowId, int JamaatId, Guid reportSectionId)
        {
            var submissionWindow = await _context.ReportSubmissions
                .Include(x => x.Answers)
                .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .ThenInclude(x => x.ReportSections)
                .FirstOrDefaultAsync(q => q.SubmissionWindowId == submissionWindowId && q.JamaatId == JamaatId && q.Answers.Any(x => x.ReportSubmissionSectionId == reportSectionId));
            return submissionWindow;
        }

        public async Task<List<ReportResponse>?> GetReportSubmittedResponse(Guid reportSubmissionId, Guid reportSectionId)
        {
            var reportResponse = await _context.ReportResponses
                .Where(r => r.ReportSubmissionId == reportSubmissionId && r.ReportSubmissionSectionId == reportSectionId)
                .ToListAsync();
            return reportResponse;
        }

        public async Task<bool> DeleteReportSubmissionAnswer(List<ReportResponse> answers)
        {
            _context.ReportResponses.RemoveRange(answers);

            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;
        }
    }
}
