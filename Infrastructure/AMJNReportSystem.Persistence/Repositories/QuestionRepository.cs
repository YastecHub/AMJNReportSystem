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
				.Where(x => x.SectionId == sectionId)
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
    }

}
