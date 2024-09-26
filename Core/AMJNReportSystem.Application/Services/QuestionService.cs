using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using AMJNReportSystem.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AMJNReportSystem.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionOptionRepository _questionOptionRepository;
        private readonly IReportSectionRepository _reportSectionRepository;
        private readonly ILogger<QuestionService> _logger;

        public QuestionService(IQuestionRepository questionRepository, IQuestionOptionRepository questionOptionRepository, IReportSectionRepository reportSectionRepository, ILogger<QuestionService> logger)
        {
            _questionRepository = questionRepository;
            _questionOptionRepository = questionOptionRepository;
            _reportSectionRepository = reportSectionRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> CreateQuestion(CreateQuestionRequest request)
        {
            try
            {
                _logger.LogInformation("Creating a new question...");

                if (request == null)
                {
                    _logger.LogWarning("Request is null. Returning error.");
                    return await Result<bool>.FailAsync("Question can't be null.");
                }

                var sectionExist = await _reportSectionRepository.GetReportSectionById(request.ReportSectionId);

                if (sectionExist == null)
                {
                    _logger.LogWarning($"Invalid ReportSectionId: {request.ReportSectionId}");
                    return await Result<bool>.FailAsync("Invalid ReportSectionId.");
                }

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

                var questionCreated = await _questionRepository.AddQuestion(question);
                if (!questionCreated)
                {
                    _logger.LogError("Failed to create the question.");
                    return await Result<bool>.FailAsync("Failed to create the question.");
                }

                bool optionsCreated = true;
                if (request.Options != null && request.Options.Count > 0)
                {
                    _logger.LogInformation("Adding options for the question...");

                    foreach (var optionRequest in request.Options)
                    {
                        var option = new QuestionOption
                        {
                            Id = Guid.NewGuid(),
                            QuestionId = question.Id,
                            Text = optionRequest.Text,
                            CreatedBy = "Admin",
                            CreatedOn = DateTime.Now,
                            IsDeleted = false
                        };

                        var optionCreated = await _questionOptionRepository.CreateQuestionOption(option);
                        if (!optionCreated)
                        {
                            _logger.LogError($"Failed to create option for Question ID: {question.Id}");
                            optionsCreated = false;
                            break;
                        }
                    }
                }

                _logger.LogInformation("Question and options created successfully.");
                if (optionsCreated && request.Options != null && request.Options.Count > 0)
                {
                    return await Result<bool>.SuccessAsync(true, "Question and options created successfully.");
                }
                else if (optionsCreated && (request.Options == null || request.Options.Count == 0))
                {
                    return await Result<bool>.SuccessAsync(true, "Question created successfully, no options provided.");
                }
                else
                {
                    return await Result<bool>.SuccessAsync(true, "Question created successfully, but one or more options failed to create.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a question: {ex.Message}");
                return await Result<bool>.FailAsync($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateQuestion(Guid questionId, UpdateQuestionRequest request)
        {
            _logger.LogInformation($"Updating question with ID: {questionId}");

            if (request is null)
            {
                _logger.LogWarning("Request is null. Returning error.");
                return await Result<bool>.FailAsync("Question can't be null.");
            }

            var question = await _questionRepository.GetQuestionById(questionId);
            if (question == null)
            {
                _logger.LogWarning($"Question not found with ID: {questionId}");
                return await Result<bool>.FailAsync("Question not found.");
            }

            question.QuestionName = request.QuestionName;
            question.QuestionType = request.QuestionType;
            question.ResponseType = request.ResponseType;
            question.IsRequired = request.IsRequired;
            question.IsActive = request.IsActive;
            question.LastModifiedBy = "Admin";
            question.LastModifiedOn = DateTime.Now;

            bool optionsUpdated = true;
            if (request.Options != null && request.Options.Count > 0)
            {
                _logger.LogInformation("Updating options for the question...");

                foreach (var option in question.Options)
                {
                    await _questionOptionRepository.DeleteQuestionOption(option);
                }

                foreach (var optionDto in request.Options)
                {
                    var questionOption = new QuestionOption
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = question.Id,
                        LastModifiedBy = "Admin",
                        LastModifiedOn = DateTime.Now,
                        Text = optionDto.Text
                    };

                    var optionCreated = await _questionOptionRepository.CreateQuestionOption(questionOption);
                    if (!optionCreated)
                    {
                        _logger.LogError($"Failed to update option for Question ID: {question.Id}");
                        optionsUpdated = false;
                        break;
                    }
                }
            }

            var questionUpdated = await _questionRepository.UpdateQuestion(question);

            if (questionUpdated && optionsUpdated)
            {
                _logger.LogInformation($"Question and options updated successfully for ID: {questionId}");
                return await Result<bool>.SuccessAsync(true, "Question and options updated successfully.");
            }
            else if (questionUpdated && !optionsUpdated)
            {
                _logger.LogWarning($"Question updated but some options failed for ID: {questionId}");
                return await Result<bool>.SuccessAsync(true, "Question updated successfully, but some options failed to update.");
            }
            else
            {
                _logger.LogError($"Failed to update question with ID: {questionId}");
                return await Result<bool>.FailAsync("Failed to update the question.");
            }
        }

        public async Task<Result<bool>> DeleteQuestion(Guid questionId)
        {
            _logger.LogInformation($"Deleting question with ID: {questionId}");

            var question = await _questionRepository.GetQuestionById(questionId);
            if (question == null)
            {
                _logger.LogWarning($"Question not found with ID: {questionId}");
                return Result<bool>.Fail("Question not found.");
            }

            question.IsActive = false;
            question.IsDeleted = true;
            question.DeletedOn = DateTime.Now;
            question.DeletedBy = "Admin";

            var result = await _questionRepository.UpdateQuestion(question);

            if (result)
            {
                _logger.LogInformation($"Question successfully deleted with ID: {questionId}");
            }
            else
            {
                _logger.LogError($"Failed to delete question with ID: {questionId}");
            }

            return result ? Result<bool>.Success(true) : Result<bool>.Fail("Failed to delete question.");
        }

        public async Task<Result<QuestionDto>> GetQuestion(Guid questionId)
        {
            _logger.LogInformation($"Fetching question with ID: {questionId}");

            var question = await _questionRepository.GetQuestionById(questionId);
            if (question == null)
            {
                _logger.LogWarning($"Question not found with ID: {questionId}");
                return Result<QuestionDto>.Fail("Question not found.");
            }

            var questionDto = new QuestionDto
            {
                Id = question.Id,
                ReportSectionId = question.SectionId,
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

            _logger.LogInformation($"Question retrieved successfully for ID: {questionId}");
            return Result<QuestionDto>.Success(questionDto, "Question retrieved successfully");
        }

        public async Task<Result<IList<QuestionDto>>> GetQuestions()
        {
            _logger.LogInformation("Fetching all active questions...");

            var questions = await _questionRepository.GetQuestions(q => q.IsActive && !q.IsDeleted);

            var questionDtos = questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                ReportSectionId = q.SectionId,
                SectionName = q.ReportSection.ReportSectionName,
                QuestionName = q.QuestionName,
                IsRequired = q.IsRequired,
                IsActive = q.IsActive,
                QuestionType = q.QuestionType,
                ResponseType = q.ResponseType,
            }).ToList();

            _logger.LogInformation("Questions retrieved successfully.");
            return Result<IList<QuestionDto>>.Success(questionDtos, "Questions retrieved successfully");
        }

        public async Task<Result<IList<QuestionDto>>> GetQuestionsBySection(Guid sectionId)
        {
            _logger.LogInformation($"Fetching questions for section ID: {sectionId}");

            var questions = await _questionRepository.GetQuestionsBySection(sectionId);
            var questionDtos = questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                ReportSectionId = q.SectionId,
                SectionName = q.ReportSection.ReportSectionName,
                QuestionName = q.QuestionName,
                IsRequired = q.IsRequired,
                IsActive = q.IsActive,
                QuestionType = q.QuestionType,
                ResponseType = q.ResponseType
            }).ToList();

            _logger.LogInformation($"Questions retrieved successfully for section ID: {sectionId}");
            return Result<IList<QuestionDto>>.Success(questionDtos, "Questions retrieved successfully");
        }
    }
}
