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
	Task<Result<IList<QuestionDto>>> GetQuestionsBySection(Guid sectionId);
	Task<Result<IList<QuestionOptionDto>>> GetQuestionOptions(Guid questionId);
	Task<Result<IList<QuestionDto>>> GetQuestions();
	Task<Result<IList<QuestionDto>>> GetQuestionsByReportTypeId(Guid reportTypeId);

    Task<BaseResponse<List<ReportTypeSectionQuestion>>> GetQuestionReportSectionByReportTypeId(Guid reportTypeId);
	Task<BaseResponse<List<ReportTypeSectionQuestionWithStatus>>> GetQuestionReportSectionByReportTypeIdSlim(Guid reportTypeId, Guid submissionWindowId);


}
