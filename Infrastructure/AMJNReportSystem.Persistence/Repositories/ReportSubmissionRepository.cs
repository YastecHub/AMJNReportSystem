using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Persistence.Extensions;
using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Models.ResponseModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class ReportSubmissionRepository : IReportSubmissionRepository
    {
        private readonly ApplicationContext _context;

        public ReportSubmissionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<ReportSubmission> AddReportSubmissionAsync(ReportSubmission reportSubmission)
        {
            await _context.AddAsync(reportSubmission);
            await _context.SaveChangesAsync();
            return reportSubmission;
        }

        public async Task<ReportSubmission> GetReportTypeSubmissionAsync(Expression<Func<ReportSubmission, bool>> expression)
        {
            var reportSubmission = await _context.ReportSubmissions.Include(x => x.ReportType).SingleOrDefaultAsync(expression);
            return reportSubmission;
        }
        public async Task<PaginatedResult<ReportSubmissionResponseModel>> GetReportTypeSubmissionsAsync(PaginationFilter filter)
        {
            var reportsSubmission = _context.ReportSubmissions;
            if (string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var response = await reportsSubmission.ToMappedPaginatedResultAsync<ReportSubmission, ReportSubmissionResponseModel>(filter.PageNumber, filter.PageSize);
                return response;
            }
            var searchResponse = await reportsSubmission.SearchByKeyword(filter.Keyword).ToMappedPaginatedResultAsync<ReportSubmission, ReportSubmissionResponseModel>(filter.PageNumber, filter.PageSize);
            return searchResponse;
        }

        public async Task<ReportSubmission> UpdateReportSubmission(ReportSubmission reportSubmission)
        {
            _context.Update(reportSubmission);
            await _context.SaveChangesAsync();
            return reportSubmission;
        }

        public async Task<bool> Exist(string reportSubmissionName)
        {
            var reportSub = await _context.ReportSubmissions.AnyAsync(x => x.ReportTag.ToString() == "");
            return reportSub;
        }
    }
}
