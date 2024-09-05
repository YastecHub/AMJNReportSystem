using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Persistence.Context;

namespace AMJNReportSystem.Persistence.Repositories
{
	public class QuestionOptionRepository : IQuestionOptionRepository
	{
		private readonly ApplicationContext _context;
		public QuestionOptionRepository(ApplicationContext context)
		{
			_context = context;
		}

		public async Task<bool> CreateQuestionOption(QuestionOption questionOption)
		{
			var question = await _context.AddAsync(questionOption);
			await _context.SaveChangesAsync();
			return true;
		}  

		public Task<IList<QuestionOption>> GetAllQuestionOptionAsync()
		{
			throw new NotImplementedException();
		}

		public Task<QuestionOption> GetQuestionOptionById(Guid Id)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> UpdateQuestionOption(QuestionOption questionOption)
		{
			var generalQuestion = _context.Update(questionOption);
			await _context.SaveChangesAsync();
			return true;

		}
	}
}
