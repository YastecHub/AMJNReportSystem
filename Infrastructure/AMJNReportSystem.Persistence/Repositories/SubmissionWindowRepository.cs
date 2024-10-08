using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
         
        public async Task<bool> UpdateSubmissionWindow(SubmissionWindow submissionWindow)
        {
            var submission = _context.Update(submissionWindow);
            _context.SaveChanges();
            return true;
        }
    }
}
