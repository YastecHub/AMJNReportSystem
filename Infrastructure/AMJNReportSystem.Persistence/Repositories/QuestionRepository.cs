using AMJNReportSystem.Persistence.Context;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Question> GetQuestionAsync(Expression<Func<Question, bool>> expression)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(expression);
            return question;
        }

        public async Task<IList<Question>> GetQuestionsBySection(Guid sectionId)
        {
            var questions = await _context.Questions.
                Where(s => s.SectionId == sectionId).ToListAsync();
            return questions;
        }

        public async Task<IList<Question>> GetQuestions(Expression<Func<Question, bool>> expression)
        {
            var questions = await _context.Questions.Include(x => x.Section).Where(expression).ToListAsync();
            return questions;
        }

        public async Task<Question> GetQuestion(Expression<Func<Question, bool>> expression)
        {
            var questions = await _context.Questions.FirstOrDefaultAsync(expression);
            return questions;
        }

        public async Task<Question> GetQuestionById(Guid id)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(q => q.Id == id);
            return question;
        }
    }
}
