using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
	public interface IQuestionOptionRepository
    {
        Task<bool> CreateQuestionOption(QuestionOption questionOption);
        Task<IList<QuestionOption>> GetAllQuestionOptionAsync();
        Task<QuestionOption> GetQuestionOptionById(Guid Id);
        Task<bool> UpdateQuestionOption(QuestionOption questionOption);
        Task<bool> DeleteQuestionOption(QuestionOption questionOption);
        Task<IList<QuestionOption>> GetQuestionOptions(Guid questionId);

	}
}
