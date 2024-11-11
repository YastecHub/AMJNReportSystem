using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AMJNReportSystem.Application.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationContext _context;
        public QuestionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> AddQuestion(Question question)
        {
            var generalQuestion = await _context.AddAsync(question);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateQuestion(Question question)
        {
            var generalQuestion = _context.Update(question);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<IList<Question>> GetQuestions(Expression<Func<Question, bool>> expression)
        {
            var questions = await _context.Questions
                .Include(x => x.ReportSection)
                .Include(x => x.Options)
                .Where(expression)
                .ToListAsync();
            return questions;
        }

        public async Task<IList<Question>> GetQuestionsBySection(Guid sectionId)
        {
            var questions = await _context.Questions
                .Include(x => x.ReportSection)
                .Include(x => x.Options)
                .Where(x => x.ReportSectionId == sectionId)
                .ToListAsync();
            return questions;
        }

        public async Task<Question> GetQuestionById(Guid id)
        {
            var questions = await _context.Questions
                .Include(x => x.ReportSection)
                .Include(x => x.Options)
                .SingleOrDefaultAsync(q => q.Id == id);
            return questions;
        }

        public List<Question> GetAllQuestion()
        {
            return _context.Questions.ToList();
        }

        public async Task<List<Question>> GetQuestionsByReportTypeId(Guid reportTypeId)
        {
            var questions = await _context.Questions
                .Include(x => x.ReportSection)
                .ThenInclude(x => x.ReportType)
                .Include(x => x.Options)
                .Where(q => q.ReportSection.ReportTypeId == reportTypeId)
                .ToListAsync();
            return questions;
        }

        public async Task<List<ReportTypeSectionQuestion>> GetQuestionReportSectionByReportTypeId(Guid reportTypeId)
        {
            var questions = await _context.ReportSections
                .Include(x => x.ReportType)
                .Include(x => x.Questions)
                .Where(q => q.ReportTypeId == reportTypeId)
                .Select(x => new ReportTypeSectionQuestion
                {
                    SectionId = x.Id,
                    SectionName = x.ReportSectionName,
                    ReportSectionQuestions = x.Questions.Select(q => new ReportSectionQuestionDto
                    {
                        Id = q.Id,
                        QuestionText = q.QuestionName,
                        IsActive = q.IsActive,
                        IsRequired = q.IsRequired,
                        Options = q.Options.Select(qo => new ReportQuestionOptionDto
                        {
                            Id = qo.Id,
                            OptionText = qo.Text
                        }).ToList(),

                    }).ToList(),
                })
                .ToListAsync();
            return questions;
        }


        public async Task<List<ReportTypeSectionQuestionSlim>> GetQuestionReportSectionByReportTypeIdSlim(Guid reportTypeId)
        {
            var questions = await _context.ReportSections
                .Where(q => q.ReportTypeId == reportTypeId)
                .Select(x => new ReportTypeSectionQuestionSlim
                {
                    SectionId = x.Id,
                    SectionName = x.ReportSectionName,
                })
                .ToListAsync();
            return questions;
        }

        public async Task<List<ReportTypeSectionQuestionWithStatus>> GetQuestionReportSectionByReportTypeIdSlim
            (Guid reportTypeId, Guid submissionWindowId, int jamaatId)
        {
            var reportTypeSection = await _context.ReportSections
                .Where(q => q.ReportTypeId == reportTypeId)
                .Select(x => new ReportTypeSectionQuestionWithStatus
                {
                    SectionId = x.Id,
                    SectionName = x.ReportSectionName,
                    IsSubmitted = false
                })
                .ToListAsync();

            foreach (var section in reportTypeSection)
            {

                var checkReportSectionChecked = await _context.ReportSubmissions
                       .Include(x => x.Answers)
                       .Where(x => !x.IsDeleted && x.SubmissionWindowId == submissionWindowId && x.JamaatId == jamaatId && x.Answers
                       .Any(x => x.ReportSubmissionSectionId == section.SectionId))
                       .FirstOrDefaultAsync();

                if(checkReportSectionChecked != null)
                {
                    section.IsSubmitted = true;
                }
            }
            return reportTypeSection;
        }
    }
}
