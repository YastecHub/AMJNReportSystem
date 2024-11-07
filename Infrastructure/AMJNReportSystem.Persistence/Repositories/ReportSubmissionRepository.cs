using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AMJNReportSystem.Application.Models.DTOs;
using System.Collections.Immutable;
using AMJNReportSystem.Application.Models.ResponseModels;
using System.Globalization;
using AMJNReportSystem.Application;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class ReportSubmissionRepository : IReportSubmissionRepository
    {
        private readonly ApplicationContext _dbcontext;
        private readonly IGatewayHandler _gatewayHandler;

        public ReportSubmissionRepository(ApplicationContext dbcontext, IGatewayHandler gatewayHandler)
        {
            _dbcontext = dbcontext;
            _gatewayHandler = gatewayHandler;
        }

        public async Task<ReportSubmission> CreateReportSubmissionAsync(ReportSubmission reportSubmission)
        {
            await _dbcontext.ReportSubmissions.AddAsync(reportSubmission);
            await _dbcontext.SaveChangesAsync();
            return reportSubmission;
        }

        public async Task<bool> Exist(string reportSubmissionName)
        {
            return await _dbcontext.ReportSubmissions
                .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .AnyAsync(x => x.SubmissionWindow.ReportType.Name == reportSubmissionName);
        }

        public async Task<ReportSubmission> GetReportTypeSubmissionByIdAsync(Guid id)
        {
            var reportSubmission = await _dbcontext.ReportSubmissions
                .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .Include(x => x.Answers)
                .ThenInclude(x => x.Question)
                .ThenInclude(x => x.Options)
                .Include(x => x.Answers)
                .ThenInclude(x => x.QuestionOption)
                .SingleOrDefaultAsync(x => x.Id == id);

            return reportSubmission;
        }

        public async Task<PaginatedResult<ReportSubmission>> GetAllReportTypeSubmissionsAsync(PaginationFilter filter)
        {
            var query = _dbcontext.ReportSubmissions
                 .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
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

        public async Task<List<ReportSubmission>> GetAllReportTypeSubmissionsAsync()
        {
            var query = _dbcontext.ReportSubmissions
                 .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Question)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.QuestionOption)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<ReportSubmission> UpdateReportSubmission(ReportSubmission reportSubmission)
        {
            _dbcontext.ReportSubmissions.Update(reportSubmission);
            await _dbcontext.SaveChangesAsync();
            return reportSubmission;
        }

        public async Task<List<ReportSubmission>> GetReportSubmissionsByReportTypeAsync(Guid reportTypeId)
        {
            var submissions = await _dbcontext.ReportSubmissions
                .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .Include(x => x.Answers)
                .ThenInclude(x => x.Question)
                .Include(x => x.Answers)
                .ThenInclude(x => x.QuestionOption)
                .Where(x => x.SubmissionWindow.ReportTypeId == reportTypeId)
                .ToListAsync();

            return submissions;
        }

        public async Task<List<ReportSubmission>> GetReportSubmissionsByCircuitIdAsync(int circuitId)
        {
            var submissions = await _dbcontext.ReportSubmissions
                   .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Question)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.QuestionOption)
                .Where(x => x.CircuitId == circuitId)
                .ToListAsync();

            return submissions;
        }

        public async Task<List<ReportSubmission>> GetReportSubmissionsByJamaatIdAsync(int jamaatId)
        {
            var submissions = await _dbcontext.ReportSubmissions
                    .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .ThenInclude(x => x.ReportSections)
                .Include(x => x.SubmissionWindow)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Question)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.QuestionOption)
                .Where(x => x.JamaatId == jamaatId)
                .ToListAsync();

            return submissions;
        }

        public List<ReportSubmission> GetAllReportSubmission()
        {
            return _dbcontext.ReportSubmissions.ToList();
        }

        public async Task<List<ReportSubmission>> GetJamaatReportsBySubmissionWindowIdAsync(Guid submissionWindowId)
        {
            var submissions = await _dbcontext.ReportSubmissions
                .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .Where(x => x.SubmissionWindowId == submissionWindowId)
                .ToListAsync();

            return submissions;
        }

        public ReportSubmissionResult GetTotalMonthlyReport(int month)
        {
            var submissions = _dbcontext.ReportSubmissions
                .Include(x => x.SubmissionWindow)
                    .ThenInclude(x => x.ReportType)
                .Include(x => x.SubmissionWindow)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Question)
                .Include(x => x.Answers)
                    .ThenInclude(x => x.QuestionOption)
                .Where(x => x.SubmissionWindow.Month == month)
                .ToList();

            return new ReportSubmissionResult
            {
                Submissions = submissions
            };
        }

        public async Task<PdfResponse> GetReportSubmission(Guid jamaatSubmissionId)
        {

            var getJamaat = await _gatewayHandler.GetListOfMuqamAsync();

            if (getJamaat == null)
                return new PdfResponse();

            var data = await _dbcontext.ReportSubmissions
                .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .ThenInclude(x => x.ReportSections)
                .ThenInclude(x => x.Questions)
                .ThenInclude(x => x.Options)
                .Where(q => q.Id == jamaatSubmissionId)
                .Select(x => new SubmittedReportDto
                {
                    JamaatId = x.JamaatId,
                    CircuitId = x.CircuitId,
                    JammatEmailAddress = x.JammatEmailAddress,
                    ReportTypeName = x.SubmissionWindow.ReportType.Name,
                    ReportTypDescription = x.SubmissionWindow.ReportType.Description,
                    Year = x.SubmissionWindow.Year,
                    Id = x.Id,
                    Month = x.SubmissionWindow.Month,
                    Answers = x.Answers.Select(x => new SubmittedReportResponseDto
                    {
                        Id = x.Id,
                        QuestionName = x.Question.QuestionName,
                        QuestionType = x.Question.QuestionType,
                        ReportSectionId = x.Question.ReportSectionId,
                        IsActive = x.Question.IsActive,
                        IsRequired = x.Question.IsRequired,
                        QuestionId = x.QuestionId,
                        TextAnswer = x.TextAnswer,
                        ResponseType = x.Question.ResponseType,
                        QuestionOptionId = x.QuestionOptionId,
                        Report = x.Report,
                        ReportSectionName = x.Question.ReportSection.ReportSectionName,
                        SubmittedQuestionOptions = x.Question.Options.Select(x => new SubmittedQuestionOption
                        {
                            Id = x.Id,
                            QuestionId = x.QuestionId,
                            Text = x.Text,
                        }).ToList(),

                    }).ToList()
                }).FirstOrDefaultAsync();

            if (data == null)
                return new PdfResponse();

            if (data.Answers == null)
                return new PdfResponse();

            var pdfReport = data.Answers
                .GroupBy(x => x.ReportSectionName)
                .Select(x => new PdfReportSection
                {

                    SectionName = x.Key,
                    Questions = x.Select(m => new PdfQuestionAnswer
                    {
                        Answer = m.TextAnswer,
                        OptionsAnswer = m.SubmittedQuestionOptions != null ? m.SubmittedQuestionOptions.Select(s => s.Text).ToList() : null,
                        Question = m.QuestionName,
                    }).ToList()

                }).ToList();


            var result = new PdfResponse()
            {
                PdfReportSections = pdfReport,
                EmailAddress = data.JammatEmailAddress,
                Month = GetMonthName(data.Month),
                ReportTypeDescription = data.ReportTypDescription,
                ReportTypeName = data.ReportTypeName,
                Year = data.Year,
                Jamaat = GetMuqamiDetailByJamaatId(getJamaat, data.JamaatId).JamaatName,
                Circuit = GetMuqamiDetailByJamaatId(getJamaat, data.CircuitId).CircuitName

            };


            return result;
        }


        public async Task<ReportSubmission?> GetReportSubmissionSectionAsync(Guid reportSubmissionWindowId, Guid reportSectionId, int jamaatId)
        {
            var reportSubmission = await _dbcontext.ReportSubmissions
                .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .Include(x => x.Answers)
                .ThenInclude(x => x.Question)
                .ThenInclude(x => x.Options)
                .Include(x => x.Answers)
                .ThenInclude(x => x.QuestionOption)
                .FirstOrDefaultAsync(x => x.SubmissionWindowId == reportSubmissionWindowId
                && x.JamaatId == jamaatId
                && x.Answers.Any(x => x.ReportSubmissionSectionId == reportSectionId));

            return reportSubmission;
        }

        public async Task<List<ReportSubmission>> GetJamaatMonthlyReport(int jamaatId, int month)
        {
            var submissions = await _dbcontext.ReportSubmissions
                .Include(x => x.SubmissionWindow)
                .ThenInclude(x => x.ReportType)
                .Include(x => x.Answers)
                .ThenInclude(x => x.Question)
                .Include(x => x.Answers)
                .ThenInclude(x => x.QuestionOption)
                .Where(x => x.JamaatId == jamaatId || x.SubmissionWindow.Month == month)
                .ToListAsync();

            return submissions;
        }

        private static (string? JamaatName, string? CircuitName) GetMuqamiDetailByJamaatId(List<Muqam> getJamaat, int jamaatId)
        {

            if (getJamaat == null || !getJamaat.Any())
            {
                return (null, null);
            }

            var detail = getJamaat.FirstOrDefault(x => x.JamaatId == jamaatId);

            return detail != null
                ? (detail.JamaatName, detail.CircuitName)
                : (null, null);
        }

        private static string GetMonthName(int monthNumber)
        {
            // Validate the month number
            if (monthNumber < 1 || monthNumber > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(monthNumber), "Month must be between 1 and 12.");
            }

            // Use DateTime to get the month name
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);
        }
    }
}
