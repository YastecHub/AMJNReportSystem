using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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
            var question = await _context.Questions.FirstOrDefaultAsync(expression);
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
            var questions = await _context.Questions.Include(x => x.ReportSection).Where(expression).ToListAsync();
            return questions;
        }

        public async Task<Question> GetQuestionById(Guid id)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(q => q.Id == id);
            return question;
        }
    }
}
