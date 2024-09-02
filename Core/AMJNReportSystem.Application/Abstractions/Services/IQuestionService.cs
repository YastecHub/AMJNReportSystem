using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
namespace AMJNReportSystem.Application.Abstractions.Services;

public interface IQuestionService
{
	Task<Result<bool>> CreateQuestion(CreateQuestionRequest request);
	Task<Result<bool>> UpdateQuestion(Guid questionId, UpdateQuestionRequest request); 
	Task<Result<bool>> DeleteQuestion(Guid questionId); 
	Task<Result<QuestionDto>> GetQuestion(Guid questionId); 
	Task<Result<IList<QuestionDto>>> GetQuestions();
	Task<IList<Question>> GetQuestionsBySection(Guid sectionId);
}
