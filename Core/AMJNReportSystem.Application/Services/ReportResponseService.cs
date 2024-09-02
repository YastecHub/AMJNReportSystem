using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Domain.Repositories;

namespace AMJNReportSystem.Application.Services
{
    public class ReportResponseService : IReportResponseService
    {
        private readonly IReportResponseRepository _repository;

        public ReportResponseService(IReportResponseRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ReportResponseDto>> GetAllReportResponsesAsync()
        {
            var responses = await _repository.GetAllReportResponseAsync();
            return responses.Select(r => new ReportResponseDto
            {
                Id = r.Id,
                QuestionId = r.QuestionId,
                Question = r.Question,
                TextAnswer = r.TextAnswer,
                QuestionOptionId = r.QuestionOptionId,
                QuestionOption = r.QuestionOption,
                Report = r.Report
            });
        }

        public async Task<ReportResponseDto?> GetReportResponseByIdAsync(Guid responseId)
        {
            var response = await _repository.GetReportResponseByIdAsync(responseId);
            if (response == null)
            {
                return null;
            }

            return new ReportResponseDto
            {
                Id = response.Id,
                QuestionId = response.QuestionId,
                Question = response.Question,
                TextAnswer = response.TextAnswer,
                QuestionOptionId = response.QuestionOptionId,
                QuestionOption = response.QuestionOption,
                Report = response.Report
            };
        }

        public async Task<ReportResponseDto> CreateReportResponseAsync(ReportResponseDto responseDto)
        {
            var response = new ReportResponse
            {
                Id = responseDto.Id,
                QuestionId = responseDto.QuestionId,
                TextAnswer = responseDto.TextAnswer,
                QuestionOptionId = responseDto.QuestionOptionId,
                Report = responseDto.Report
            };

            var createdResponse = await _repository.AddReportResponseAsync(response);

            return new ReportResponseDto
            {
                Id = createdResponse.Id,
                QuestionId = createdResponse.QuestionId,
                Question = createdResponse.Question,
                TextAnswer = createdResponse.TextAnswer,
                QuestionOptionId = createdResponse.QuestionOptionId,
                QuestionOption = createdResponse.QuestionOption,
                Report = createdResponse.Report
            };
        }

        public async Task<ReportResponseDto> UpdateReportResponseAsync(ReportResponseDto responseDto)
        {
            var response = new ReportResponse
            {
                Id = responseDto.Id,
                QuestionId = responseDto.QuestionId,
                TextAnswer = responseDto.TextAnswer,
                QuestionOptionId = responseDto.QuestionOptionId,
                Report = responseDto.Report
            };

            var updatedResponse = await _repository.UpdateReportResponseAsync(response);

            return new ReportResponseDto
            {
                Id = updatedResponse.Id,
                QuestionId = updatedResponse.QuestionId,
                Question = updatedResponse.Question,
                TextAnswer = updatedResponse.TextAnswer,
                QuestionOptionId = updatedResponse.QuestionOptionId,
                QuestionOption = updatedResponse.QuestionOption,
                Report = updatedResponse.Report
            };
        }

        public async Task<bool> DeleteReportResponseAsync(Guid responseId)
        {
            return await _repository.DeleteReportResponseAsync(responseId);
        }
    }
}
