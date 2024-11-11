using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Domain.Entities;
using System.Linq.Expressions;

namespace AMJNReportSystem.Application.Abstractions.Repositories;

public interface IQuestionRepository
{
	Task<bool> AddQuestion(Question question);
	Task<bool> UpdateQuestion(Question question);
	Task<IList<Question>> GetQuestionsBySection(Guid sectionId);
	Task<IList<Question>> GetQuestions(Expression<Func<Question, bool>> expression);
	Task<Question> GetQuestionById(Guid id);
    List<Question> GetAllQuestion();
	Task<List<Question>> GetQuestionsByReportTypeId(Guid reportTypeId);
	Task<List<ReportTypeSectionQuestion>> GetQuestionReportSectionByReportTypeId(Guid reportTypeId);
	Task<List<ReportTypeSectionQuestionSlim>> GetQuestionReportSectionByReportTypeIdSlim(Guid reportTypeId);
	Task<List<ReportTypeSectionQuestionWithStatus>> GetQuestionReportSectionByReportTypeIdSlim(Guid reportTypeId, Guid submissionWindowId,int jamaatId);
}
