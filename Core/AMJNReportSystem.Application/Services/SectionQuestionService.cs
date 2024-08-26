using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Wrapper;
using Domain.Entities;
using Mapster;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace AMJNReportSystem.Application.Services
{
    public class SectionQuestionService : ISectionQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IReportTypeRepository _reportTypeRepository;

        public SectionQuestionService(IQuestionRepository questionRepository, ISectionRepository sectionRepository, IReportTypeRepository reportTypeRepository)
        {
            _questionRepository = questionRepository;
            _sectionRepository = sectionRepository;
            _reportTypeRepository = reportTypeRepository;
        }

        public async Task<Result<bool>> AddQuestion(ReportQuestionRequest request)
        {
            var question = request.Adapt<Question>();
            await _questionRepository.AddQuestion(question);
            return await Result<bool>.SuccessAsync("Question Successfully added");
        }

        public async Task<Result<QuestionDto>> GetQuestion(Guid questionId)
        {
            var question = await _questionRepository.GetQuestion(x => x.Id == questionId);

            if (question is null) return await Result<QuestionDto>.FailAsync("Question with provided Id not found");

            var questionDto = question.Adapt<QuestionDto>();
            questionDto.ResponseType = question.ResponseType.ToString();
            return await Result<QuestionDto>.SuccessAsync(questionDto, "Successfully retrieved question");
        }


        public async Task<Result<ReportQuestionsModel>> GetReportTypeQuestions(Guid reportTypeId)
        {
            var reportType = await _reportTypeRepository.GetReportTypeById(reportTypeId);
            var sections = await _sectionRepository.GetSectionsByReportType(reportTypeId);

            var response = new ReportQuestionsModel
            {
                ReportTypeId = reportTypeId,
                ReportTypeName = reportType.Name,
                Sections = new List<Sections>()
            };

            foreach (var section in sections)
            {
                var sectionQuestions = await ReportSectionQuestions(section.Id);
                response.Sections.Add(new Sections
                {
                    SectionId = section.Id,
                    Title = section.Name,
                    Questions = sectionQuestions
                });
            }
            return await Result<ReportQuestionsModel>.SuccessAsync(response, "Successfully retrieved question");
        }

        private async Task<IList<ReportSectionQuestion>> ReportSectionQuestions(Guid sectionId)
        {
            var questions = await _questionRepository.GetQuestions(x => x.SectionId == sectionId);
            var response = questions.Adapt<IList<ReportSectionQuestion>>();
            return response;
        }

        public async Task<Result<IEnumerable<QuestionDto>>> GetSectionQuestions(Guid sectionId)
        {
            var questions = await _questionRepository.GetQuestions(x => x.SectionId == sectionId);

            if (questions.Count is 0) return await Result<IEnumerable<QuestionDto>>.FailAsync("There is no question for the provided section Id");
            var questionDtos = questions.Select(x => x.Adapt<QuestionDto>());
            /*var questionDtos = new List<QuestionDto>();
            for(int i = 0; i < questions.Count(); i++)
            {
                var questionDto = new QuestionDto
                {
                    Points = questions[i].Points,
                    ResponseType = questions[i].ResponseType.ToString(),
                    SectionId = questions[i].SectionId,
                    Text = questions[i].Text
                };
                questionDtos.Add(questionDto);
            }*/

            return await Result<IEnumerable<QuestionDto>>.SuccessAsync(questionDtos, "Successfully retrieved question");
        }

        public async Task<Result<bool>> QuestionActivenessState(Guid questionId, bool state)
        {
            var question = await _questionRepository.GetQuestionById(questionId);
            if (question is null)
                return await Result<bool>.FailAsync("Input question Id not found");
            question.isActive = state;
            await _questionRepository.UpdateQuestion(question);
            return await Result<bool>.SuccessAsync("Question Activeness state updated");

        }

        public async Task<Result<bool>> UpdateQuestionPoint(Guid questionId, double point)
        {
            var question = await _questionRepository.GetQuestion(x => x.Id == questionId);

            if (question is null) return await Result<bool>.FailAsync("Question with provided Id not found");
            question.Points = point;
            await _questionRepository.UpdateQuestion(question);
            return await Result<bool>.SuccessAsync("Successfully retrieved question");
        }

        public async Task<Result<bool>> UpdateQuestionText(Guid questionId, string text)
        {
            var question = await _questionRepository.GetQuestion(x => x.Id == questionId);

            if (question is null) return await Result<bool>.FailAsync("Question with provided Id not found");
            question.Text = text;
            await _questionRepository.UpdateQuestion(question);
            return await Result<bool>.SuccessAsync("Successfully retrieved question");
        }
    }
}
