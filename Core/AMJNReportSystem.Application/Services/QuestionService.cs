using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Application.Services
{
	public class QuestionService : IQuestionService
	{
		private readonly IQuestionRepository _questionRepository;

		public QuestionService(IQuestionRepository questionRepository)
		{
			_questionRepository = questionRepository;
		}

		public async Task<Result<bool>> CreateQuestion(CreateQuestionRequest request)
		{
			if (request is null)
				return await Result<bool>.FailAsync("Question can't be null.");
			var question = new Question
			{
				Id = Guid.NewGuid(),
				QuestionName = request.QuestionName,
				QuestionType = request.QuestionType,
				ResponseType = request.ResponseType,
				IsRequired = request.IsRequired,
				IsActive = request.IsActive,
				SectionId = request.ReportSectionId,
				CreatedBy = "Admin",
				CreatedOn = DateTime.Now,
			};

			var result = await _questionRepository.AddQuestion(question);

			return result ? Result<bool>.Success(true) : Result<bool>.Fail("Failed to create question.");
		}

		public async Task<Result<bool>> UpdateQuestion(Guid questionId, UpdateQuestionRequest request)
		{
			if (request is null)
				return await Result<bool>.FailAsync("Question can't be null.");
			var question = await _questionRepository.GetQuestionById(questionId);
			if (question == null)
			{
				return Result<bool>.Fail("Question not found.");
			}

			question.QuestionName = request.QuestionName;
			question.QuestionType = request.QuestionType;
			question.ResponseType = request.ResponseType;
			question.IsRequired = request.IsRequired;
			question.IsActive = request.IsActive;
			question.LastModifiedBy = "Admin";
			question.LastModifiedOn = DateTime.Now;
			question.Options = request.Options.Select(o => new QuestionOption
			{
				QuestionId = questionId,
				Text = o.Text
			}).ToList();

			var result = await _questionRepository.UpdateQuestion(question);

			return result ? Result<bool>.Success(true) : Result<bool>.Fail("Failed to update question.");
		}

		public async Task<Result<bool>> DeleteQuestion(Guid questionId)
		{
			var question = await _questionRepository.GetQuestionById(questionId);
			if (question == null)
			{
				return Result<bool>.Fail("Question not found.");
			}

			question.IsActive = false;
			question.IsDeleted = true;
			question.DeletedOn = DateTime.Now;
			question.DeletedBy = "Admin";

			var result = await _questionRepository.UpdateQuestion(question);

			return result ? Result<bool>.Success(true) : Result<bool>.Fail("Failed to delete question.");
		}

		public async Task<Result<QuestionDto>> GetQuestion(Guid questionId)
		{
			var question = await _questionRepository.GetQuestionById(questionId);
			if (question == null)
			{
				return Result<QuestionDto>.Fail("Question not found.");
			}

			var questionDto = new QuestionDto
			{
				Id = question.Id,
				SectionName = question.ReportSection.ReportSectionName,
				QuestionName = question.QuestionName,
				IsRequired = question.IsRequired,
				IsActive = question.IsActive,
				QuestionType = question.QuestionType,
				ResponseType = question.ResponseType,
				Options = question.Options.Select(o => new QuestionOption
				{
					Text = o.Text
				}).ToList()
			};

			return Result<QuestionDto>.Success(questionDto);
		}

		public async Task<Result<IList<QuestionDto>>> GetQuestions()
		{
			var questions = await _questionRepository.GetQuestions(q => q.IsActive && !q.IsDeleted);

			var questionDtos = questions.Select(q => new QuestionDto
			{
				Id = q.Id,
				SectionName = q.ReportSection.ReportSectionName,
				QuestionName = q.QuestionName,
				IsRequired = q.IsRequired,
				IsActive = q.IsActive,
				QuestionType = q.QuestionType,
				ResponseType = q.ResponseType,
				Options = q.Options.Select(o => new QuestionOption
				{
					Text = o.Text
				}).ToList()
			}).ToList();

			return Result<IList<QuestionDto>>.Success(questionDtos);
		}

		public async Task<IList<Question>> GetQuestionsBySection(Guid sectionId)
		{
			return await _questionRepository.GetQuestionsBySection(sectionId);
		}
	}
}
