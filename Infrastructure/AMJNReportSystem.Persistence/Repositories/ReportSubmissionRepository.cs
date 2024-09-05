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
        private readonly ApplicationContext _dbcontext;

        public ReportSubmissionRepository(ApplicationContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<ReportSubmission> CreateReportSubmissionAsync(ReportSubmission reportSubmission)
        {
            await _dbcontext.AddAsync(reportSubmission);
            await _dbcontext.SaveChangesAsync();
            return reportSubmission;
        }

        

        public async Task<bool> Exist(string reportSubmissionName)
        {
            return await _dbcontext.ReportSubmissions
                .AnyAsync(x => x.ReportType.Title == reportSubmissionName);
        }


        public async Task<ReportSubmission> GetReportTypeSubmissionByIdAsync(Guid id)
        {
            var reportSubmission = await _dbcontext.ReportSubmissions
                .Include(x => x.ReportType)
                .Include(x => x.SubmissionWindow)
                .Include(x => x.Answers)
                .ThenInclude(x => x.Question)
                .Include(x => x.Answers)
                .ThenInclude(x => x.QuestionOption)
                .SingleOrDefaultAsync(x => x.Id == id);

            return reportSubmission;
        }

        public async Task<PaginatedResult<ReportSubmission>> GetAllReportTypeSubmissionsAsync(PaginationFilter filter)
        {
            var query = _dbcontext.ReportSubmissions
                .Include(x => x.ReportType)
                .Include(x => x.SubmissionWindow)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Question)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.QuestionOption)
                .AsQueryable();
            var totalCount = await query.CountAsync();

            var submissions = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PaginatedResult<ReportSubmission>
            {
                TotalCount = totalCount,
                Data = submissions
            };
        }


        public async Task<ReportSubmission> UpdateReportSubmission(ReportSubmission reportSubmission)
        {
            _dbcontext.ReportSubmissions.Update(reportSubmission);
            await _dbcontext.SaveChangesAsync();
            return reportSubmission;
        }

    }
}
