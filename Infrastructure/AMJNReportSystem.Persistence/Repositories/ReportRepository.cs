using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Persistence.Extensions;
using Application.Abstractions.Repositories;
using Application.Models;
using Application.Wrapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationContext _context;

        public ReportRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Report> AddAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<PaginatedResult<Report>> GetReportsByReporter(Guid reporterId, PaginationFilter filter)
        {
            return await _context.Reports.Include(rep => rep.Reporter)
           .Where(report => report.ReporterId == reporterId)
            .AsQueryable().ToPaginatedListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<PaginatedResult<Report>> GetReportsByReportSubmission(Guid reportSubmssionId, PaginationFilter filter)
        {
            return await _context.Reports.Include(rep => rep.Reporter)
            .AsQueryable().ToPaginatedListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<PaginatedResult<Report>> GetReportsByReportSubmissionAndReporter(Guid reportSubmissionId, Guid reporterId, PaginationFilter filter)
        {
            return await _context.Reports
            .AsQueryable().ToPaginatedListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<PaginatedResult<Report>> GetSectionReportsByReportSubmission(Guid reportSubmissionId, Guid sectionId, PaginationFilter filter)
        {
            return await _context.Reports
            .ToPaginatedListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<IReadOnlyList<Report>> GetSelectedReportsByReportSubmission(Guid reportSubmissionId, IList<Guid> reportIds)
        {
            return await _context.Reports
             .AsQueryable()
             .ToListAsync();
        }

        public async Task<Report> UpdateAsync(Report report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
            return report;
        }
    }
}
