﻿using AMJNReportSystem.Domain.Entities;
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

        public async Task<IEnumerable<SubmissionWindow>> GetAllSubmissionWindowsAsync(Guid? reportTypeId, int? month, int? year, bool? isLocked, DateTime? startDate, DateTime? endDate)
        {
            var getAllSubmissionWindows = await _context.SubmissionWindows.Where(x => x.ReportTypeId == reportTypeId && x.Month == month && x.Year == year && x.StartingDate == startDate && x.EndingDate == endDate && isLocked == false).ToListAsync();
            return getAllSubmissionWindows;
        }

        public async Task<SubmissionWindow> GetSubmissionWindowAsync(Expression<Func<SubmissionWindow, bool>> expression)
        {
            var submissionWindow = await _context.SubmissionWindows.SingleOrDefaultAsync(expression);
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
